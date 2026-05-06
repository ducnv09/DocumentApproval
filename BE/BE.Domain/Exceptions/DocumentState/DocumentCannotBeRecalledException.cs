using System;

namespace BE.Domain.Exceptions.DocumentState
{
    public class DocumentCannotBeRecalledException : DomainException
    {
        public DocumentCannotBeRecalledException() 
            : base("Không thể thu hồi tờ trình này (đã có người duyệt hoặc không ở trạng thái chờ).")
        {
        }

        public DocumentCannotBeRecalledException(string message) 
            : base(message)
        {
        }
    }
}
