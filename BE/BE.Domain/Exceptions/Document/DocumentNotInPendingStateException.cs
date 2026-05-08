namespace BE.Domain.Exceptions
{
    public class DocumentNotInPendingStateException : DomainException
    {
        public DocumentNotInPendingStateException(string message = "Tờ trình không ở trạng thái chờ phê duyệt.") 
            : base(message)
        {
        }
    }
}
