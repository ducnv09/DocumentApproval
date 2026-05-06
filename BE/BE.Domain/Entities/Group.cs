using System;
using System.Collections.Generic;

namespace BE.Domain.Entities
{
    public class Group
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected Group() { }

        public Group(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(code)) 
            {
                throw new ArgumentNullException(nameof(code));
            }

            Id = Guid.NewGuid();
            Name = name;
            Code = code;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) 
            {
                throw new ArgumentNullException(nameof(newName));
            }
            Name = newName;
        }
    }
}
