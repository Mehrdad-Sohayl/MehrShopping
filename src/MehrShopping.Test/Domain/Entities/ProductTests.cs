using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.Errors;

namespace MehrShopping.Test.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Create_ValidParameters_ShouldReturnProduct()
        {
            // Arrange
            var name = "Test Product";
            var quantity = 10;

            // Act
            var product = Product.Create(name, quantity);

            // Assert
            product.Should().NotBeNull();
            product.Name.Value.Should().Be(name);
            product.StockQuantity.Value.Should().Be(quantity);
        }

        [Theory]
        [InlineData("", 10)]
        [InlineData("ValidName", -5)]
        [InlineData(null, 0)]
        public void Create_InvalidParameters_ShouldThrowDomainException(string name, int quantity)
        {
            // Act & Assert
            Action act = () => Product.Create(name, quantity);
            act.Should().Throw<MehrShopping.Domain.Exceptions.DomainException>();
        }

        [Fact]
        public void DeleteProduct_WhenStockGreaterThanZero_ShouldThrowDomainException()
        {
            var product = Product.Create("Prod", 5);
            Action act = () => product.DeleteProduct(1);
            act.Should().Throw<MehrShopping.Domain.Exceptions.DomainException>();
        }

        [Fact]
        public void DecreaseStock_WhenSufficientStock_ShouldReduceQuantity()
        {
            var product = Product.Create("Prod", 10);
            product.DecreaseStock(3);
            product.StockQuantity.Value.Should().Be(7);
        }

        [Fact]
        public void DecreaseStock_WhenInsufficientStock_ShouldThrowDomainException()
        {
            var product = Product.Create("Prod", 2);
            Action act = () => product.DecreaseStock(5);
            act.Should().Throw<MehrShopping.Domain.Exceptions.DomainException>();
        }
        [Fact]
        public void UpdateStockQuantity_WithPositiveValue_ShouldChangeStockQuantity()
        {
            var product = Product.Create("Test", 10);
            product.UpdateStockQuantity(20);
            product.StockQuantity.Should().Be(20m);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void UpdateStockQuantity_WithNonPositiveValue_ShouldThrowDomainException(int invalid)
        {
            var product = Product.Create("Test", 10);
            Action act = () => product.UpdateStockQuantity(invalid);
            act.Should().Throw<DomainException>()
                .Which.Errors.First().Code.Should().Be(DomainErrorCodes.InvalidQuantity);
        }
    }
}
