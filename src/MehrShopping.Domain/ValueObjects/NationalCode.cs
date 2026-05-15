using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Domain.ValueObjects
{
    public class NationalCode
    {
        public string Value { get; }

        private NationalCode(string value)
        {
            Value = value;
        }

        public static NationalCode Create(string nationalCode, List<DomainError> errors)
        {
            if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode.Length != 10 || !nationalCode.All(char.IsDigit))
            {
                errors.Add(new DomainError(DomainErrorCodes.InvalidNationalCode, nameof(nationalCode)));
                return null;
            }

            return new NationalCode(nationalCode);

        }
    }
}
