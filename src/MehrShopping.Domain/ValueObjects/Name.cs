using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Domain.ValueObjects
{
    public class Name
    {
        public string Value { get; }

        private Name(string value)
        {
            Value = value;
        }

        public static Name Create(string value, List<DomainError> errors)
        {
            if (string.IsNullOrEmpty(value))
            {
                errors.Add(new DomainError(DomainErrorCodes.InvalidName, nameof(value)));
                return null;
            }

            return new Name(value);
        }
    }
}
