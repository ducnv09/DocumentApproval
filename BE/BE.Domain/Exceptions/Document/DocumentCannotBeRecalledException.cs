namespace BE.Domain.Exceptions
{
    public class DocumentCannotBeRecalledException : DomainException
    {
        public DocumentCannotBeRecalledException(string message = "Không thể thu hồi tờ trình ở trạng thái hiện tại.") 
            : base(message)
        {
        }
    }
}
