using Xunit;
using FluentAssertions;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;

namespace MehrShopping.Test.Domain.Entities
{
    public class InvoiceTests
    {
        [Fact]
        public void AddItem_WithValidData_ShouldIncreaseTotal()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var invoice = Invoice.Create(customer);
            var product = Product.Create("Test", 10);
            var item = InvoiceItem.Create(product, 2);
            invoice.AddItem(item);
            var a = invoice.Items
                 .Where(ii => ii.ProductId == product.Id).FirstOrDefault()
                 .Quantity.Value.Should().Be(2);
        }

        [Fact]
        public void AddItem_DuplicateProduct_ShouldThrowDomainException()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var invoice = Invoice.Create(customer);
            var product = Product.Create("Test", 10);
            var item1 = InvoiceItem.Create(product, 2);
            invoice.AddItem(item1);
            Action act = () => invoice.AddItem(InvoiceItem.Create(product, 1));
            act.Should().Throw<DomainException>()
                .WithMessage("Product already added to the invoice.");
        }

        [Fact]
        public void AddItem_WithInvalidQuantity_ShouldThrowDomainException()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var invoice = Invoice.Create(customer);
            var product = Product.Create("Test", 10);
            Action act = () => invoice.AddItem(InvoiceItem.Create(product, -1));
            act.Should().Throw<DomainException>()
                .WithMessage("Quantity must be positive.");
        }

        [Fact]
        public void AddItem_WithInvalidPrice_ShouldThrowDomainException()
        {
            var customer = Customer.Create("John", "Doe", "123456789");
            var invoice = Invoice.Create(customer);
            var product = Product.Create("Test", -10);
            Action act = () => invoice.AddItem(InvoiceItem.Create(product, 2));
            act.Should().Throw<DomainException>()
                .WithMessage("Price must be positive.");
        }
    }
}

