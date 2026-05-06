using System;

namespace BE.Domain.Exceptions.NotFound
{
    public class DocumentNotFoundException : DomainException
    {
        public DocumentNotFoundException(Guid id) 
            : base($"Không tìm thấy tờ trình với ID: {id}")
        {
        }

        public DocumentNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
