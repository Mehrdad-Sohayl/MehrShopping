using MehrShopping.Domain.Errors;

namespace MehrShopping.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public IReadOnlyList<DomainError> Errors { get; } = new List<DomainError>();

        public DomainException()
        {
        }

        public DomainException(string? message, List<DomainError> errors) : base(message)
        {
            Errors = errors;
        }

        public DomainException(List<DomainError> errors)
        {
            Errors = errors;
        }

        public DomainException(DomainError error)
        {

            Errors = new List<DomainError>() { error };
        }
    }

    public sealed record DomainError(string Code, string Message);
}
