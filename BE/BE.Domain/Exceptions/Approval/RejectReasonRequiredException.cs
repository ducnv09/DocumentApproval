namespace BE.Domain.Exceptions
{
    public class RejectReasonRequiredException : DomainException
    {
        public RejectReasonRequiredException(string message = "Lý do từ chối là bắt buộc.") 
            : base(message)
        {
        }
    }
}
