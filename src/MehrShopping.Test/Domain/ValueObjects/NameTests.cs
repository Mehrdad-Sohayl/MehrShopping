using Xunit;
using FluentAssertions;
using MehrShopping.Domain.ValueObjects;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Test.Domain.ValueObjects
{
    public class NameTests
    {
        [Fact]
        public void Constructor_WithValidName_ShouldCreateInstance()
        {
            var errors = new List<DomainError>();
            var name = "John Doe";
            var nv = Name.Create(name, errors);
            nv.Should().NotBeNull();
            nv.Value.Should().Be(name.Trim());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithInvalidName_ShouldThrowDomainException(string invalid)
        {
            var errors = new List<DomainError>();
            Action act = () => Name.Create(invalid, errors);
            act.Should().Throw<DomainException>()
                .WithMessage("Name cannot be empty.");
        }
    }
}
