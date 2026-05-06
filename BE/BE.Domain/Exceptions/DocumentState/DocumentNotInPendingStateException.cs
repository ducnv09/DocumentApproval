using System;

namespace BE.Domain.Exceptions.DocumentState
{
    public class DocumentNotInPendingStateException : DomainException
    {
        public DocumentNotInPendingStateException() 
            : base("Tờ trình không nằm ở trạng thái chờ duyệt.")
        {
        }

        public DocumentNotInPendingStateException(string message) 
            : base(message)
        {
        }
    }
}
