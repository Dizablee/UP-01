using EuroLink.Models;

namespace EuroLink.Services
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Phone? Phone { get; set; }

        public static OperationResult Success(Phone phone)
        {
            return new OperationResult
            {
                IsSuccess = true,
                Phone = phone
            };
        }

        public static OperationResult Failure(string errorMessage)
        {
            return new OperationResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}