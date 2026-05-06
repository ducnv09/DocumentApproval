using System;

namespace BE.Domain.Entities
{
    public class Position
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        protected Position() { }

        public Position(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = Guid.NewGuid();
            Name = name;
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
