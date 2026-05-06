using System;

namespace BE.Domain.Exceptions.Concurrency
{
    public class ApprovalConcurrencyException : DomainException
    {
        public ApprovalConcurrencyException() 
            : base("Tờ trình đã được xử lý bởi một người khác trước đó.")
        {
        }

        public ApprovalConcurrencyException(string message) 
            : base(message)
        {
        }
    }
}
