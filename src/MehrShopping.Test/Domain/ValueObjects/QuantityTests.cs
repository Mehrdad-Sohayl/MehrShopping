using FluentAssertions;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;
using Xunit;

namespace MehrShopping.Test.Domain.ValueObjects
{
    public class QuantityTests
    {
        [Fact]
        public void Constructor_WithPositiveValue_ShouldCreateInstance()
        {
            // Arrange
            var value = 5;
            var errors = new List<DomainError>();
            // Act
            var quantity = Quantity.Create(value, errors);
            // Assert
            quantity.Should().NotBeNull();
            quantity.Value.Should().Be(value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithNonPositiveValue_ShouldThrowDomainException(int invalid)
        {
            // Act
            var errors = new List<DomainError>();
            Action act = () => Quantity.Create(invalid, errors);
            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}
