namespace BE.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string entityName, object key) 
            : base($"{entityName} ({key}) không tồn tại.")
        {
        }
    }
}
