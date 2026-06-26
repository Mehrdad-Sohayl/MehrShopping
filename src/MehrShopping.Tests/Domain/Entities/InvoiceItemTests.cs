using FluentAssertions;
using Xunit;

namespace MehrShopping.Tests.Domain.Entities
{
    public class InvoiceItemTests
    {
        [Fact]
        public void Should_Create_Invoice_Item_With_Valid_Data()
        {
            // Arrange
            var item = new InvoiceItem(product: null!, quantity: 2, price: 199.99m);

            // Assert
            item.Quantity.Should().Be(2);
            item.Price.Should().Be(199.99m);
        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Product()
        {
            // Act
            Action act = () => new InvoiceItem(product: null!, quantity: 2, price: 199.99m);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Product cannot be null");
        }
    }
}