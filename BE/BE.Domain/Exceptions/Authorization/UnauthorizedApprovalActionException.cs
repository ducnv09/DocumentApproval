using System;

namespace BE.Domain.Exceptions.Authorization
{
    public class UnauthorizedApprovalActionException : DomainException
    {
        public UnauthorizedApprovalActionException() 
            : base("Bạn không có quyền thực hiện hành động phê duyệt tại bước này.")
        {
        }

        public UnauthorizedApprovalActionException(string message) 
            : base(message)
        {
        }
    }
}
