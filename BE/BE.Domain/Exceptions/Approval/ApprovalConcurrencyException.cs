namespace BE.Domain.Exceptions
{
    public class ApprovalConcurrencyException : DomainException
    {
        public ApprovalConcurrencyException(string message = "Tờ trình đã được xử lý bởi người khác. Vui lòng tải lại trang.") 
            : base(message)
        {
        }
    }
}
