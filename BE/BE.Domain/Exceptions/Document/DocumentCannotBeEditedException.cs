namespace BE.Domain.Exceptions
{
    public class DocumentCannotBeEditedException : DomainException
    {
        public DocumentCannotBeEditedException(string message = "Tờ trình không thể chỉnh sửa ở trạng thái hiện tại.") 
            : base(message)
        {
        }
    }
}
