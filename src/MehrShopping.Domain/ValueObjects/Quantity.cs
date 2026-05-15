using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Domain.ValueObjects
{
    public class Quantity
    {
        public int Value { get; }

        private Quantity(int value) => Value = value;

        public static Quantity Create(int value, List<DomainError> errors)
        {
            if (value < 0)
            {
                errors.Add(new DomainError(DomainErrorCodes.InvalidQuantity, nameof(value)));
                return null;
            }

            return new Quantity(value);

        }
    }
}
