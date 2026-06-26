using FluentAssertions;
using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;
using Xunit;

namespace MehrShopping.Tests.Domain.ValueObjects;

public class NameTests
{
    [Fact]
    public void Create_WithValidValue_ShouldReturnName()
    {
        var errors = new List<DomainError>();

        var result = Name.Create("Ali", errors);

        result.Should().NotBeNull();
        result.Value.Should().Be("Ali");
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithNullValue_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = Name.Create(null, errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithEmptyValue_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = Name.Create("", errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_ShouldAddInvalidNameErrorCode()
    {
        var errors = new List<DomainError>();

        Name.Create(null, errors);

        errors[0].Code.Should().Be(DomainErrorCodes.InvalidName);
        errors[0].Message.Should().Be("value");
    }

    [Fact]
    public void Create_ShouldMutateProvidedErrorList()
    {
        var errors = new List<DomainError>();

        Name.Create(null, errors);
        Name.Create("", errors);

        errors.Count.Should().Be(2);
    }
}