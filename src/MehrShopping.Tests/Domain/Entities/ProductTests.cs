using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using Xunit;

namespace MehrShopping.Tests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateProduct()
        {
            // Arrange & Act
            var product = Product.Create("Laptop", 5);

            // Assert
            product.Should().NotBeNull();
            product.Name.Value.Should().Be("Laptop");
            product.StockQuantity.Value.Should().Be(5);
        }

        [Fact]
        public void Create_WithNullName_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create(null!, 5);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create(string.Empty, 5);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create("Laptop", -1);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithZeroQuantity_ShouldSucceed()
        {
            // Act
            var product = Product.Create("Laptop", 0);

            // Assert
            product.StockQuantity.Value.Should().Be(0);
        }

        [Fact]
        public void DeleteProduct_WithZeroStock_ShouldNotThrow()
        {
            // Arrange
            var product = Product.Create("Laptop", 0);

            // Act & Assert
            product.Invoking(p => p.DeleteProduct(product.Id)).Should().NotThrow();
        }

        [Fact]
        public void DeleteProduct_WithPositiveStock_ShouldThrowDomainException()
        {
            // Arrange
            var product = Product.Create("Laptop", 5);

            // Act & Assert
            product.Invoking(p => p.DeleteProduct(product.Id))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void DecreaseStock_WithSufficientStock_ShouldDecreaseQuantity()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act
            product.DecreaseStock(4);

            // Assert
            product.StockQuantity.Value.Should().Be(6);
        }

        [Fact]
        public void DecreaseStock_WithExactStock_ShouldSetStockToZero()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act
            product.DecreaseStock(10);

            // Assert
            product.StockQuantity.Value.Should().Be(0);
        }

        [Fact]
        public void DecreaseStock_WithInsufficientStock_ShouldThrowDomainException()
        {
            // Arrange
            var product = Product.Create("Laptop", 5);

            // Act & Assert
            product.Invoking(p => p.DecreaseStock(6))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void DecreaseStock_WithZeroQuantity_ShouldNotChangeStock()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act
            product.DecreaseStock(0);

            // Assert
            product.StockQuantity.Value.Should().Be(10);
        }

        [Fact]
        public void DecreaseStock_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act & Assert
            product.Invoking(p => p.DecreaseStock(-1))
                .Should().Throw<DomainException>();
        }
    }
}
