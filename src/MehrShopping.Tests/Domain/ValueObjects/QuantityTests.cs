using FluentAssertions;
using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;

namespace MehrShopping.Tests.Domain.ValueObjects;

public class QuantityTests
{
    [Fact]
    public void Create_WithPositiveValue_ShouldReturnQuantity()
    {
        var errors = new List<DomainError>();

        var result = Quantity.Create(10, errors);

        result.Should().NotBeNull();
        result.Value.Should().Be(10);
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithZero_ShouldReturnQuantity()
    {
        var errors = new List<DomainError>();

        var result = Quantity.Create(0, errors);

        result.Should().NotBeNull();
        result.Value.Should().Be(0);
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithNegativeValue_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = Quantity.Create(-1, errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithNegativeValue_ShouldAddInvalidQuantityError()
    {
        var errors = new List<DomainError>();

        Quantity.Create(-5, errors);

        errors[0].Code.Should().Be(DomainErrorCodes.InvalidQuantity);
        errors[0].Message.Should().Be("value");
    }

    [Fact]
    public void Create_ShouldMutateErrorList()
    {
        var errors = new List<DomainError>();

        Quantity.Create(-1, errors);
        Quantity.Create(-2, errors);

        errors.Count.Should().Be(2);
    }
}