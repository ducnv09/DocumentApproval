namespace BE.Domain.Exceptions
{
    public class UnauthorizedApprovalActionException : DomainException
    {
        public UnauthorizedApprovalActionException(string message = "Bạn không có quyền thực hiện hành động phê duyệt tại bước này.") 
            : base(message)
        {
        }
    }
}
