namespace BE.Domain.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid id) 
            : base("Người dùng", id)
        {
        }

        public UserNotFoundException(string username) 
            : base("Người dùng", username)
        {
        }
    }
}
