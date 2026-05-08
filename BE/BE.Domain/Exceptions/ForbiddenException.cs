namespace BE.Domain.Exceptions
{
    public class ForbiddenException : DomainException
    {
        public ForbiddenException(string message = "Bạn không có quyền thực hiện hành động này.") 
            : base(message)
        {
        }
    }
}
