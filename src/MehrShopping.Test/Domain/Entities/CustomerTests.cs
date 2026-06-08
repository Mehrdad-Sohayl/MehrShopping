using Xunit;
using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.ValueObjects;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.Errors;

namespace MehrShopping.Test.Domain.Entities
{
    public class CustomerTests
    {
        [Fact]
        public void CreateValidCustomer_ShouldReturnCustomer()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var errors = new List<DomainError>();
            customer.FirstName.Should().BeEquivalentTo(Name.Create("John", errors));
            customer.LastName.Should().BeEquivalentTo(Name.Create("Doe", errors));
            customer.NationalCode.Should().BeEquivalentTo(NationalCode.Create("123456789", errors));
        }

        [Fact]
        public void CreateInvalidFirstName_ShouldThrowDomainException()
        {
            var ex = Assert.Throws<DomainException>(() => Customer.Create("", "Doe", "123456789"));
            ex.Errors.Should().ContainSingle(e => e.Code == DomainErrorCodes.InvalidName);
        }

        [Fact]
        public void CreateInvalidLastName_ShouldThrowDomainException()
        {
            var ex = Assert.Throws<DomainException>(() => Customer.Create("John", "", "123456789"));
            ex.Errors.Should().ContainSingle(e => e.Code == DomainErrorCodes.InvalidName);
        }

        [Fact]
        public void CreateInvalidNationalCode_ShouldThrowDomainException()
        {
            var ex = Assert.Throws<DomainException>(() => Customer.Create("John", "Doe", "invalidcode"));
            ex.Errors.Should().ContainSingle(e => e.Code == DomainErrorCodes.InvalidNationalCode);
        }

        [Fact]
        public void UpdateValid_ShouldUpdateCustomerWithoutException()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var errors = new List<DomainError>();
            customer.Update("Jane", "Smith");
            customer.FirstName.Should().BeEquivalentTo(Name.Create("Jane", errors));
            customer.LastName.Should().BeEquivalentTo(Name.Create("Smith", errors));
        }

        [Fact]
        public void UpdateInvalid_ShouldThrowDomainException()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var ex = Assert.Throws<DomainException>(() => customer.Update("", "Smith"));
            ex.Errors.Should().ContainSingle(e => e.Code == DomainErrorCodes.InvalidName);
        }
    }
}
