namespace BE.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors) 
            : base("Một hoặc nhiều lỗi xác thực đã xảy ra.")
        {
            // Sử dụng OrdinalIgnoreCase để tránh phân biệt hoa thường khi truy xuất lỗi
            Errors = new Dictionary<string, string[]>(errors, StringComparer.OrdinalIgnoreCase);
        }

        public ValidationException(string propertyName, string errorMessage)
            : base("Một hoặc nhiều lỗi xác thực đã xảy ra.")
        {
            Errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { propertyName, new[] { errorMessage } }
            };
        }
    }
}
