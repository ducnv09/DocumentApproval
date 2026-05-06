using System;

namespace BE.Domain.Exceptions.Authorization
{
    public class UserInactiveAccountException : DomainException
    {
        public UserInactiveAccountException() 
            : base("Tài khoản người dùng hiện đang bị khóa hoặc không hoạt động.")
        {
        }

        public UserInactiveAccountException(string message) 
            : base(message)
        {
        }
    }
}
