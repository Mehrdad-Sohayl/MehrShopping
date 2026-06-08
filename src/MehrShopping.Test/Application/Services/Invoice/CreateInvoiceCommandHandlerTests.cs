using Moq;
using FluentAssertions;
using MehrShopping.Application.Services.Invoice.Commands;
using MehrShopping.Application.Interfaces;
using MehrShopping.Domain.Interfaces.Repositories;
using CustomerEntity = MehrShopping.Domain.Entities.Customer;
using ProductEntity = MehrShopping.Domain.Entities.Product;
using InvoiceEntity = MehrShopping.Domain.Entities.Invoice;

namespace MehrShopping.Test.Application.Services.Invoice
{
    public class CreateInvoiceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenCustomerAndInvoiceItemExists_ShouldCreateInvoice()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var items = new List<CreateInvoiceItem>()
            {
                new CreateInvoiceItem(1, 10)
            };

            var cmd = new CreateInvoiceCommand(1, items);

            var customer = CustomerEntity.Create("First", "Last", "1234567890");
            var product = ProductEntity.Create("Test", 10);

            customerRepositoryMock
                .Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(customer);

            productRepositoryMock
                .Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(product);

            invoiceRepositoryMock
                .Setup(i => i.AddAsync(It.IsAny<InvoiceEntity>()));

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync());

            var handler = new CreateInvoiceCommandHandler(
                customerRepositoryMock.Object, productRepositoryMock.Object, invoiceRepositoryMock.Object, unitOfWorkMock.Object);

            var result = await handler.Handle(cmd);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            customerRepositoryMock.Verify(c => c.GetByIdAsync(It.IsAny<int>()), Times.Once);
            productRepositoryMock.Verify(p => p.GetByIdAsync(It.IsAny<int>()), Times.Once);
            invoiceRepositoryMock.Verify(i => i.AddAsync(It.IsAny<InvoiceEntity>()), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);

        }
    }
}
