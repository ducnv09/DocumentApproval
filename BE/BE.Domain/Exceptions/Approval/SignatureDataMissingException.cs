namespace BE.Domain.Exceptions
{
    public class SignatureDataMissingException : DomainException
    {
        public SignatureDataMissingException(string message = "Dữ liệu chữ ký bị thiếu.") 
            : base(message)
        {
        }
    }
}
