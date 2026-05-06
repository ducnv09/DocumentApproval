using System;

namespace BE.Domain.Entities
{
    public class Step
    {
        public Guid Id { get; private set; }
        public Guid WorkflowId { get; private set; }
        public int StepOrder { get; private set; }
        public Guid GroupId { get; private set; }
        public Guid? PositionId { get; private set; }
        public int ApprovalCount { get; private set; }

        protected Step() { }

        public Step(Guid workflowId, int stepOrder, Guid groupId, Guid? positionId, int approvalCount = 1)
        {
            Id = Guid.NewGuid();
            WorkflowId = workflowId;
            StepOrder = stepOrder;
            GroupId = groupId;
            PositionId = positionId;
            ApprovalCount = approvalCount;
        }

        public void UpdateConfig(int stepOrder, Guid groupId, Guid? positionId, int approvalCount)
        {
            StepOrder = stepOrder;
            GroupId = groupId;
            PositionId = positionId;
            ApprovalCount = approvalCount;
        }
    }
}
