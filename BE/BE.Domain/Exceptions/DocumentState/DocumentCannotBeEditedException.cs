using System;

namespace BE.Domain.Exceptions.DocumentState
{
    public class DocumentCannotBeEditedException : DomainException
    {
        public DocumentCannotBeEditedException() 
            : base("Tờ trình đang ở trạng thái không thể chỉnh sửa (chờ duyệt hoặc đã duyệt).")
        {
        }

        public DocumentCannotBeEditedException(string message) 
            : base(message)
        {
        }
    }
}
