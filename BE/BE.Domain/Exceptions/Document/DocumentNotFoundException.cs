namespace BE.Domain.Exceptions
{
    public class DocumentNotFoundException : NotFoundException
    {
        public DocumentNotFoundException(Guid id) 
            : base("Tờ trình", id)
        {
        }
    }
}
