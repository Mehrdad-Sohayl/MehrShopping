using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using Xunit;

namespace MehrShopping.Tests.Domain.Entities;

public class CustomerTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateCustomer()
    {
        var customer = Customer.Create("Ali", "Ahmadi", "1234567890");

        customer.Should().NotBeNull();
        customer.FirstName.Value.Should().Be("Ali");
        customer.LastName.Value.Should().Be("Ahmadi");
        customer.NationalCode.Value.Should().Be("1234567890");
    }

    [Fact]
    public void Create_WithInvalidFirstName_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create(null, "Ahmadi", "1234567890");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_UsingInvalidLastName_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create("Ali", null, "1234567890");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_WithInvalidNationalCode_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create("Ali", "Ahmadi", "invalid");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_WithMultipleInvalidFields_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create(null, null, "invalid");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateNames()
    {
        var customer = Customer.Create("Ali", "Ahmadi", "1234567890");

        customer.Update("Reza", "Karimi");

        customer.FirstName.Value.Should().Be("Reza");
        customer.LastName.Value.Should().Be("Karimi");
    }

    [Fact]
    public void Update_WithInvalidFirstName_ShouldThrowDomainException()
    {
        var customer = Customer.Create("Ali", "Ahmadi", "1234567890");

        Action act = () => customer.Update(null, "Karimi");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Update_WithInvalidLastName_ShouldThrowDomainException()
    {
        var customer = Customer.Create("Ali", "Ahmadi", "1234567890");

        Action act = () => customer.Update("Reza", null);

        act.Should().Throw<DomainException>();
    }
}