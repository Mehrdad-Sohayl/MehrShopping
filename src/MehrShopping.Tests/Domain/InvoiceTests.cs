using Xunit;
using FluentAssertions;
using MehrShopping.Domain.Entities;
using System.Collections.Generic;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.Errors;

namespace MehrShopping.Tests.Domain
{
    public class InvoiceTests
    {
        // Helper method to create customer with proper parameters
        private static Customer CreateCustomer(string firstName, string lastName, string nationalCode)
        {
            return Customer.Create(firstName, lastName, nationalCode);
        }

        [Fact]
        public void Constructor_WithNullCustomer_ShouldThrowDomainException()
        {
            // Act & Assert
            Action act = () => new Invoice(null);
            act.Should().Throw<DomainException>().Which.Errors.Should().ContainSingle(e => 
                e.Code == DomainErrorCodes.InvalidCustomer && e.Message == nameof(Customer));
        }

        [Fact]
        public void Constructor_WithValidCustomer_ShouldInitializeCorrectly()
        {
            // Arrange
            var customer = CreateCustomer("Test", "Customer", "1234567890");

            // Act
            var invoice = new Invoice(customer);

            // Assert
            invoice.Customer.Should().BeSameAs(customer);
            invoice.Items.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void Create_WithValidCustomer_ShouldReturnNewInstance()
        {
            // Arrange
            var customer = CreateCustomer("Test", "Customer", "1234567890");

            // Act
            var invoice = Invoice.Create(customer);

            // Assert
            invoice.Should().NotBeNull();
            invoice.Customer.Should().BeSameAs(customer);
            invoice.Items.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void AddItem_WithNullItem_ShouldThrowDomainException()
        {
            // Arrange
            var invoice = new Invoice(CreateCustomer("Test", "User", "1234567890"));

            // Act & Assert
            Action act = () => invoice.AddItem(null);
            act.Should().Throw<DomainException>().Which.Errors.Should().ContainSingle(e =>
                e.Code == DomainErrorCodes.InvalidItem && e.Message == nameof(InvoiceItem));
        }

        [Fact]
        public void AddItem_WithDuplicateProduct_ShouldThrowDomainException()
        {
            // Arrange
            var invoice = new Invoice(CreateCustomer("Test", "User", "1234567890"));
            
            var product1 = Product.Create("Test Product", 10);
            var item1 = InvoiceItem.Create(product1, 2);
            var item2 = InvoiceItem.Create(product1, 1);

            invoice.AddItem(item1);

            // Act & Assert
            Action act = () => invoice.AddItem(item2);
            act.Should().Throw<DomainException>().Which.Errors.Should().ContainSingle(e =>
                e.Code == DomainErrorCodes.DuplicateInvoiceItem && e.Message == nameof(Product));
        }

        [Fact]
        public void AddItem_WithInvalidQuantity_ShouldThrowDomainException()
        {
            // Arrange
            var invoice = new Invoice(CreateCustomer("Test", "User", "1234567890"));
            var product = Product.Create("Test Product", 10);
            var item = InvoiceItem.Create(product, 0);

            // Act & Assert
            Action act = () => invoice.AddItem(item);
            act.Should().Throw<DomainException>().Which.Errors.Should().ContainSingle(e =>
                e.Code == DomainErrorCodes.InvalidQuantity && e.Message == nameof(Quantity));
        }

        [Fact]
        public void AddItem_WithValidItem_ShouldAddToCollection()
        {
            // Arrange
            var invoice = new Invoice(CreateCustomer("Test", "User", "1234567890"));
            var product = Product.Create("Test Product", 10);
            var item = InvoiceItem.Create(product, 2);

            // Act
            invoice.AddItem(item);

            // Assert
            invoice.Items.Should().HaveCount(1);
            invoice.Items.First().Should().BeSameAs(item);
        }

        [Fact]
        public void AddItem_WithMultipleValidItems_ShouldAddAll()
        {
            // Arrange
            var invoice = new Invoice(CreateCustomer("Test", "User", "1234567890"));
            
            var product1 = Product.Create("Product 1", 10);
            var item1 = InvoiceItem.Create(product1, 2);
            
            var product2 = Product.Create("Product 2", 20);
            var item2 = InvoiceItem.Create(product2, 1);

            // Act
            invoice.AddItem(item1);
            invoice.AddItem(item2);

            // Assert
            invoice.Items.Should().HaveCount(2);
            invoice.Items[0].Should().BeSameAs(item1);
            invoice.Items[1].Should().BeSameAs(item2);
        }
    }
}