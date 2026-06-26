using Xunit;
using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Tests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateProduct()
        {
            // Arrange
            var name = "Test Product";
            int quantity = 10;

            // Act
            var product = Product.Create(name, quantity);

            // Assert
            product.Should().NotBeNull();
            product.Name.Value.Should().Be(name);
            product.StockQuantity.Value.Should().Be(quantity);
        }

        [Fact]
        public void Create_WithNullName_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create(null, 10);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create(string.Empty, 10);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create("Test", -5);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithZeroQuantity_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => Product.Create("Test", 0);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void DeleteProduct_WhenStockIsZero_ShouldNotThrow()
        {
            // Arrange
            var product = Product.Create("Test Product", 0);

            // Act
            product.DeleteProduct(1);

            // Assert
            // No exception should be thrown
        }

        [Fact]
        public void DeleteProduct_WhenStockIsPositive_ShouldThrowException()
        {
            // Arrange
            var product = Product.Create("Test Product", 5);

            // Act & Assert
            product.Invoking(p => p.DeleteProduct(1))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void DecreaseStock_WhenStockIsSufficient_ShouldDecreaseQuantity()
        {
            // Arrange
            var product = Product.Create("Test Product", 10);
            int quantityToDecrease = 5;

            // Act
            product.DecreaseStock(quantityToDecrease);

            // Assert
            product.StockQuantity.Value.Should().Be(5);
        }

        [Fact]
        public void DecreaseStock_WhenStockIsInsufficient_ShouldThrowException()
        {
            // Arrange
            var product = Product.Create("Test Product", 5);
            int quantityToDecrease = 10;

            // Act & Assert
            product.Invoking(p => p.DecreaseStock(quantityToDecrease))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void DecreaseStock_WithNegativeQuantity_ShouldThrowException()
        {
            // Arrange
            var product = Product.Create("Test Product", 10);
            int quantityToDecrease = -5;

            // Act & Assert
            product.Invoking(p => p.DecreaseStock(quantityToDecrease))
                .Should().Throw<DomainException>();
        }

        [Fact]
        public void DecreaseStock_WithZeroQuantity_ShouldNotThrow()
        {
            // Arrange
            var product = Product.Create("Test Product", 10);
            int quantityToDecrease = 0;

            // Act
            product.DecreaseStock(quantityToDecrease);

            // Assert
            product.StockQuantity.Value.Should().Be(10);
        }

        [Fact]
        public void DecreaseStock_WithExactMatch_ShouldSetToZero()
        {
            // Arrange
            var product = Product.Create("Test Product", 5);
            int quantityToDecrease = 5;

            // Act
            product.DecreaseStock(quantityToDecrease);

            // Assert
            product.StockQuantity.Value.Should().Be(0);
        }
    }
}