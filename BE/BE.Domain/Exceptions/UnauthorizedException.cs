namespace BE.Domain.Exceptions
{
    public class UnauthorizedException : DomainException
    {
        public UnauthorizedException(string message = "Tên đăng nhập hoặc mật khẩu không chính xác.") 
            : base(message)
        {
        }
    }
}
