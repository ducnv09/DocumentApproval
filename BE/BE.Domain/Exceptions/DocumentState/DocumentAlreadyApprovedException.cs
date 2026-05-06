using System;

namespace BE.Domain.Exceptions.DocumentState
{
    public class DocumentAlreadyApprovedException : DomainException
    {
        public DocumentAlreadyApprovedException() 
            : base("Tờ trình này đã được phê duyệt trước đó và không thể thay đổi.")
        {
        }

        public DocumentAlreadyApprovedException(string message) 
            : base(message)
        {
        }
    }
}
