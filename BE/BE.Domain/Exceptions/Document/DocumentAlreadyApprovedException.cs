namespace BE.Domain.Exceptions
{
    public class DocumentAlreadyApprovedException : DomainException
    {
        public DocumentAlreadyApprovedException(string message = "Tờ trình này đã được phê duyệt hoàn tất.") 
            : base(message)
        {
        }
    }
}
