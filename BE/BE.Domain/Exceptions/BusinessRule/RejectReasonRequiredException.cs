using System;

namespace BE.Domain.Exceptions.BusinessRule
{
    public class RejectReasonRequiredException : DomainException
    {
        public RejectReasonRequiredException() 
            : base("Bắt buộc phải cung cấp lý do khi từ chối tờ trình.")
        {
        }

        public RejectReasonRequiredException(string message) 
            : base(message)
        {
        }
    }
}
