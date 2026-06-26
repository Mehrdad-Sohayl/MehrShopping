using FluentAssertions;
using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;

namespace MehrShopping.Tests.Domain.ValueObjects;

public class NationalCodeTests
{
    [Fact]
    public void Create_WithValidNationalCode_ShouldReturnValueObject()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create("1234567890", errors);

        result.Should().NotBeNull();
        result.Value.Should().Be("1234567890");
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithNull_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create(null, errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithEmptyString_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create("", errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create("          ", errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithLessThan10Digits_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create("123456789", errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithMoreThan10Digits_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create("12345678901", errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_WithNonDigitCharacters_ShouldReturnNull_AndAddError()
    {
        var errors = new List<DomainError>();

        var result = NationalCode.Create("12345abcde", errors);

        result.Should().BeNull();
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Create_ShouldAddInvalidNationalCodeErrorCode()
    {
        var errors = new List<DomainError>();

        NationalCode.Create("invalid", errors);

        errors[0].Code.Should().Be(DomainErrorCodes.InvalidNationalCode);
        errors[0].Message.Should().Be("nationalCode");
    }

    [Fact]
    public void Create_ShouldMutateErrorList()
    {
        var errors = new List<DomainError>();

        NationalCode.Create("invalid", errors);
        NationalCode.Create("123", errors);

        errors.Count.Should().Be(2);
    }
}
