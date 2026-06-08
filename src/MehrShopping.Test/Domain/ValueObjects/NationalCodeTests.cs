using Xunit;
using FluentAssertions;
using MehrShopping.Domain.ValueObjects;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Test.Domain.ValueObjects
{
    public class NationalCodeTests
    {
        [Fact]
        public void Constructor_WithValidCode_ShouldCreateInstance()
        {
            var errors = new List<DomainError>();
            var code = "1234567890";
            var nationalCode = NationalCode.Create(code, errors);
            nationalCode.Should().NotBeNull();
            nationalCode.Value.Should().Be(code);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc1234567")]
        [InlineData("12345")]
        public void Constructor_WithInvalidCode_ShouldThrowDomainException(string invalid)
        {
            var errors = new List<DomainError>();

            Action act = () => NationalCode.Create(invalid, errors);
            act.Should().Throw<DomainException>()
                .WithMessage("National code is not valid.");
        }
    }
}
