using System;

namespace BE.Domain.Exceptions.BusinessRule
{
    public class SignatureDataMissingException : DomainException
    {
        public SignatureDataMissingException() 
            : base("Dữ liệu chữ ký bị thiếu khi thực hiện phê duyệt.")
        {
        }

        public SignatureDataMissingException(string message) 
            : base(message)
        {
        }
    }
}
