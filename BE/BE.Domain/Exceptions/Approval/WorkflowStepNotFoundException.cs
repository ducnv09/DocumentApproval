namespace BE.Domain.Exceptions
{
    public class WorkflowStepNotFoundException : NotFoundException
    {
        public WorkflowStepNotFoundException(Guid stepId) 
            : base("Bước quy trình", stepId)
        {
        }
    }
}
