using System;

namespace BE.Domain.Exceptions.NotFound
{
    public class WorkflowStepNotFoundException : DomainException
    {
        public WorkflowStepNotFoundException(Guid id) 
            : base($"Không tìm thấy bước duyệt với ID: {id}")
        {
        }

        public WorkflowStepNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
