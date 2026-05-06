using BE.Domain.Enums;
using System;
using System.Collections.Generic;

namespace BE.Domain.Entities
{
    public class Workflow
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public WorkflowType Type { get; private set; }
        public bool IsActive { get; private set; }

        public virtual ICollection<Step> Steps { get; private set; }

        protected Workflow()
        {
            Steps = new List<Step>();
        }

        public Workflow(string name, WorkflowType type) : this()
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void UpdateName(string newName)
        {
            Name = newName;
        }
    }
}
