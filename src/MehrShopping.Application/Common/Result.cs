using System.Xml;

namespace MehrShopping.Application.Common
{
    public class Result<T>
    {
        public T Value { get; private set; }
        public bool IsSuccess { get; private set; }
        public IReadOnlyCollection<ApplicationError> Errors { get; set; } = new List<ApplicationError>();

        public static Result<T> Success(T value)
        {
            return new Result<T>
            {
                Value = value,
                IsSuccess = true
            };
        }

        public static Result<T> Failure(ApplicationError error)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Errors = new List<ApplicationError>() { error }
            };
        }
    }

    public record ApplicationError(string Code, string Message);
}
