using System;

namespace BE.Domain.Exceptions.NotFound
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid id) 
            : base($"Không tìm thấy người dùng với ID: {id}")
        {
        }

        public UserNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
