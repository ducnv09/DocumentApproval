using System;

namespace BE.Domain.Entities
{
    public class DocType
    {
        public Guid Id { get; private set; }
        public Guid WorkflowId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        protected DocType() { }

        public DocType(Guid workflowId, string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = Guid.NewGuid();
            WorkflowId = workflowId;
            Name = name;
            Description = description;
        }

        public void UpdateInfo(string name, string? description)
        {
            Name = name;
            Description = description;
        }
    }
}
