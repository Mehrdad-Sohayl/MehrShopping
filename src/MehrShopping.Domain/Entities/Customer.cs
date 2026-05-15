using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;

namespace MehrShopping.Domain.Entities
{
    public class Customer
    {
        public Name FirstName { get; private set; }
        public Name LastName { get; private set; }
        public NationalCode NationalCode { get; private set; }

        private Customer(Name firstName, Name lastName, NationalCode nationalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            NationalCode = nationalCode;
        }

        public static Customer Create(string firstName, string lastName, string nationalCode)
        {
            var errors = new List<DomainError>();
            var _firstName = Name.Create(firstName, errors);
            var _lastName = Name.Create(lastName, errors);
            var _nationalCode = NationalCode.Create(nationalCode, errors);

            if (errors.Any())
                throw new DomainException(errors);

            return new Customer(_firstName, _lastName, _nationalCode);
        }

        public void Update(string firstName, string lastName)
        {
            var errors = new List<DomainError>();

            FirstName = Name.Create(firstName, errors);
            LastName = Name.Create(lastName, errors);

            if (errors.Any())
                throw new DomainException(errors);
        }

        #region EF

        public int Id { get; private set; }

        private Customer() { }

        #endregion
    }
}
