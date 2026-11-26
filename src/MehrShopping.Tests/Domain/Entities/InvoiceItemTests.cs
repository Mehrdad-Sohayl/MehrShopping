using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using Xunit;

namespace MehrShopping.Tests.Domain.Entities
{
    public class InvoiceItemTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateItem()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act
            var item = InvoiceItem.Create(product, 3);

            // Assert
            item.Should().NotBeNull();
            item.Product.Should().BeSameAs(product);
            item.Quantity.Value.Should().Be(3);
        }

        [Fact]
        public void Create_WithNullProduct_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => InvoiceItem.Create(null!, 3);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithNegativeQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act & Assert
            Action act = () => InvoiceItem.Create(product, -1);
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_WithZeroQuantity_ShouldSucceed()
        {
            // Arrange
            var product = Product.Create("Laptop", 10);

            // Act
            var item = InvoiceItem.Create(product, 0);

            // Assert
            item.Quantity.Value.Should().Be(0);
        }
    }
}
