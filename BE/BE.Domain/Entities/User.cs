using System;
using System.Collections.Generic;

namespace BE.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsActive { get; private set; }
        public string PasswordHash { get; private set; }

        protected User() { }

        public User(string username, string fullName, string email, bool isAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(username)) 
            {
                throw new ArgumentNullException(nameof(username));
            }

            Id = Guid.NewGuid();
            Username = username;
            FullName = fullName;
            Email = email;
            IsAdmin = isAdmin;
            IsActive = true;
        }

        public void SetPassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void UpdateInfo(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }
    }
}
