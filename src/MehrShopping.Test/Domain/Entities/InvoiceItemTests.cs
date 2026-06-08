using Xunit;
using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.Errors;

namespace MehrShopping.Test.Domain.Entities
{
    public class InvoiceItemTests
    {
        [Fact]
        public void Create_WithValidProductAndQuantity_ShouldCreateInvoiceItem()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            var invoiceItem = InvoiceItem.Create(product, 2);

            // Assert
            invoiceItem.Should().NotBeNull();
            invoiceItem.Product.Should().Be(product);
            invoiceItem.Quantity.Should().NotBeNull();
            invoiceItem.Quantity.Value.Should().Be(2);
        }

        [Fact]
        public void Create_WithNullProduct_ShouldThrowDomainException()
        {
            // Arrange
            Product product = null!;

            // Act
            Action act = () => InvoiceItem.Create(product, 2);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .Which.Errors.First().Code
                .Should().Be(DomainErrorCodes.ProductNotFound);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Create_WithInvalidQuantity_ShouldThrowDomainException(int quantity)
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            Action act = () => InvoiceItem.Create(product, quantity);

            // Assert
            act.Should()
                .Throw<DomainException>();
        }

        [Fact]
        public void Create_WithNullProductAndInvalidQuantity_ShouldThrowDomainException()
        {
            // Arrange
            Product product = null!;

            // Act
            Action act = () => InvoiceItem.Create(product, 0);

            // Assert
            act.Should()
                .Throw<DomainException>();
        }

        private static Product CreateValidProduct()
        {
            return Product.Create("Test", 10);
        }
    }
}
