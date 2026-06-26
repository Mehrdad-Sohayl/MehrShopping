using FluentAssertions;
using Xunit;

namespace MehrShopping.Tests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Should_Create_Product_With_Valid_Data()
        {
            // Arrange
            var product = new Product("Laptop", 999.99m);

            // Assert
            product.Name.Should().Be("Laptop");
            product.Price.Should().Be(999.99m);
        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Name()
        {
            // Act
            Action act = () => new Product(null, 999.99m);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Name cannot be null");
        }
    }
}