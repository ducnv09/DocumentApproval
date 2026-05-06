using BE.Domain.Enums;
using BE.Domain.Exceptions;
using BE.Domain.Exceptions.DocumentState;
using BE.Domain.Exceptions.BusinessRule;
using System;
using System.Collections.Generic;

namespace BE.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public Guid GroupId { get; private set; }
        public Guid DocTypeId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string? AttachmentUrl { get; private set; }
        public DocumentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Navigation properties (Dành cho EF Core)
        public virtual ICollection<Approval> Approvals { get; private set; }

        // Constructor mặc định cho EF Core
        protected Document()
        {
            Approvals = new List<Approval>();
        }

        public Document(Guid creatorId, Guid groupId, Guid docTypeId, string title, string content, string? attachmentUrl) : this()
        {
            Id = Guid.NewGuid();
            CreatorId = creatorId;
            GroupId = groupId;
            DocTypeId = docTypeId;
            Title = title;
            Content = content;
            AttachmentUrl = attachmentUrl;
            Status = DocumentStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        // --- BUSINESS BEHAVIORS ---
        public void SubmitForApproval()
        {
            EnsureNotApproved();

            if (Status != DocumentStatus.Draft)
            {
                throw new DocumentCannotBeEditedException("Chỉ có thể gửi duyệt các tờ trình đang ở trạng thái Nháp.");
            }

            Status = DocumentStatus.Pending;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            EnsureNotApproved();

            if (Status != DocumentStatus.Pending)
                throw new DocumentNotInPendingStateException();

            Status = DocumentStatus.Approved;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject(string reason)
        {
            EnsureNotApproved();

            if (string.IsNullOrWhiteSpace(reason))
                throw new RejectReasonRequiredException();

            if (Status != DocumentStatus.Pending)
                throw new DocumentNotInPendingStateException();

            Status = DocumentStatus.Rejected;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDraft(string title, string content, string? attachmentUrl)
        {
            EnsureNotApproved();

            if (Status != DocumentStatus.Draft && Status != DocumentStatus.Rejected)
                throw new DocumentCannotBeEditedException();

            Title = title;
            Content = content;
            AttachmentUrl = attachmentUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Recall()
        {
            EnsureNotApproved();

            if (Status != DocumentStatus.Pending)
                throw new DocumentCannotBeRecalledException("Tờ trình không ở trạng thái chờ duyệt.");

            Status = DocumentStatus.Draft;
            UpdatedAt = DateTime.UtcNow;
        }

        private void EnsureNotApproved()
        {
            if (Status == DocumentStatus.Approved)
            {
                throw new DocumentAlreadyApprovedException();
            }
        }
    }
}
