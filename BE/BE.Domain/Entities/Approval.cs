using BE.Domain.Enums;
using BE.Domain.Exceptions;
using BE.Domain.Exceptions.BusinessRule;
using BE.Domain.Exceptions.Concurrency;
using System;

namespace BE.Domain.Entities
{
    public class Approval
    {
        public Guid Id { get; private set; }
        public Guid DocumentId { get; private set; }
        public Guid StepId { get; private set; }
        public Guid GroupId { get; private set; }
        public Guid? ApproverId { get; private set; }
        public ActionType ActionType { get; private set; }
        public string? Reason { get; private set; }
        public byte[]? SignatureData { get; private set; }
        public DateTime? ActionAt { get; private set; }

        protected Approval() { }

        public static Approval CreatePending(Guid documentId, Guid stepId, Guid groupId)
        {
            return new Approval
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                StepId = stepId,
                GroupId = groupId,
                ActionType = ActionType.Pending
            };
        }

        public void ProcessApproval(Guid approverId, ActionType action, string? reason, byte[]? signature)
        {
            if (ActionType != ActionType.Pending)
                throw new ApprovalConcurrencyException();

            if (action == ActionType.Rejected && string.IsNullOrWhiteSpace(reason))
                throw new RejectReasonRequiredException();

            if (action == ActionType.Approved && (signature == null || signature.Length == 0))
                throw new SignatureDataMissingException();

            ApproverId = approverId;
            ActionType = action;
            Reason = reason;
            SignatureData = signature;
            ActionAt = DateTime.UtcNow;
        }
    }
}
