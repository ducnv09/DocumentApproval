using BE.Application.Interfaces;
using BE.Domain.Entities;
using BE.Domain.Enums;
using BE.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Application.Services
{
    public class WorkflowExecutionService : IWorkflowExecutionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkflowExecutionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task StartWorkflowAsync(Guid documentId, Guid workflowId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdAsync(documentId, cancellationToken);
            if (document == null) throw new Exception("Document not found");

            var workflow = await _unitOfWork.Workflows.GetByIdWithStepsAsync(workflowId, cancellationToken);
            if (workflow == null) throw new Exception("Workflow not found");

            document.SubmitForApproval();

            var steps = workflow.Steps.OrderBy(s => s.StepOrder).ToList();
            if (!steps.Any())
            {
                document.Approve();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return;
            }

            if (workflow.Type == WorkflowType.Sequential)
            {
                var firstStep = steps.First();
                var approval = Approval.CreatePending(documentId, firstStep.Id, firstStep.GroupId, firstStep.PositionId);
                await _unitOfWork.Approvals.AddAsync(approval, cancellationToken);
            }
            else if (workflow.Type == WorkflowType.Parallel)
            {
                foreach (var step in steps)
                {
                    var approval = Approval.CreatePending(documentId, step.Id, step.GroupId, step.PositionId);
                    await _unitOfWork.Approvals.AddAsync(approval, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task ProcessApprovalAsync(Guid documentId, Guid approverId, ActionType action, string? reason, byte[]? signature, CancellationToken cancellationToken = default)
        {
            // Sử dụng Serializable transaction để xử lý First Come First Served (Concurrency Control)
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var document = await _unitOfWork.Documents.GetByIdAsync(documentId, cancellationToken);
                if (document == null) throw new Exception("Document not found");

                if (document.Status != DocumentStatus.Pending)
                    throw new Exception("Document is not in pending state");

                var pendingApprovals = await _unitOfWork.Approvals.GetPendingByDocumentIdAsync(documentId, cancellationToken);
                if (!pendingApprovals.Any()) throw new Exception("No pending approvals found");

                // Giả định UserGroups repository trả về danh sách group của user (có thể cần viết thêm hàm hoặc list in memory nếu unitofwork chưa hỗ trợ trực tiếp)
                // Tuy nhiên hiện tại IUnitOfWork có UserGroups, ta có thể phải fetch all or có hàm riêng.
                // Để đơn giản ta giả sử UserRepository/GroupRepository có hàm GetByUserId, nhưng hiện IUserRepository chỉ trả User.
                // IUserGroupRepository.cs có lẽ có hàm. Ta sẽ dùng _unitOfWork.UserGroups
                var userGroups = await _unitOfWork.UserGroups.GetByUserIdAsync(approverId, cancellationToken);

                // Tìm step mà user hiện tại có quyền duyệt
                var targetApproval = pendingApprovals.FirstOrDefault(pa => 
                    userGroups.Any(ug => ug.GroupId == pa.GroupId && 
                                         (pa.PositionId == null || pa.PositionId == ug.PositionId)));

                if (targetApproval == null) throw new Exception("User is not authorized to approve this document at this step");

                targetApproval.ProcessApproval(approverId, action, reason, signature);
                await _unitOfWork.Approvals.UpdateAsync(targetApproval, cancellationToken);

                if (action == ActionType.Rejected)
                {
                    document.Reject(reason ?? "Bị từ chối");
                    foreach (var pa in pendingApprovals.Where(a => a.Id != targetApproval.Id))
                    {
                        pa.Cancel();
                        await _unitOfWork.Approvals.UpdateAsync(pa, cancellationToken);
                    }
                }
                else if (action == ActionType.Approved)
                {
                    var step = await _unitOfWork.Workflows.GetStepByIdAsync(targetApproval.StepId, cancellationToken);
                    if (step == null) throw new Exception("Step not found");

                    var workflow = await _unitOfWork.Workflows.GetByIdWithStepsAsync(step.WorkflowId, cancellationToken);
                    if (workflow == null) throw new Exception("Workflow not found");

                    if (workflow.Type == WorkflowType.Sequential)
                    {
                        var steps = workflow.Steps.OrderBy(s => s.StepOrder).ToList();
                        var currentStep = steps.First(s => s.Id == targetApproval.StepId);
                        var nextStep = steps.FirstOrDefault(s => s.StepOrder > currentStep.StepOrder);
                        
                        if (nextStep != null)
                        {
                            var newApproval = Approval.CreatePending(documentId, nextStep.Id, nextStep.GroupId, nextStep.PositionId);
                            await _unitOfWork.Approvals.AddAsync(newApproval, cancellationToken);
                        }
                        else
                        {
                            document.Approve();
                        }
                    }
                    else if (workflow.Type == WorkflowType.Parallel)
                    {
                        var allStepGroupIds = workflow.Steps.Select(s => s.GroupId).Distinct().ToList();
                        
                        var approvedRecords = await _unitOfWork.Approvals.GetApprovedByDocumentIdAsync(documentId, cancellationToken);
                        if (targetApproval.ActionType == ActionType.Approved && !approvedRecords.Any(a => a.Id == targetApproval.Id))
                        {
                            approvedRecords.Add(targetApproval);
                        }

                        var approvedGroupIds = approvedRecords.Select(a => a.GroupId).Distinct().ToList();
                        var allApproved = allStepGroupIds.All(gid => approvedGroupIds.Contains(gid));
                        if (allApproved)
                        {
                            document.Approve();
                        }
                    }
                }

                await _unitOfWork.Documents.UpdateAsync(document, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
