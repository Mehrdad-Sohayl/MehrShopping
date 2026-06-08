using FluentAssertions;
using MehrShopping.Application.Interfaces;
using MehrShopping.Application.Services.Invoice.Queries;
using Moq;
using ApplicationException = MehrShopping.Application.Exceptions.ApplicationException;

namespace MehrShopping.Test.Application.Queries.InvoiceList;

public class InvoiceListQueryHandlerTests
{
    private readonly Mock<IInvoiceReadRepository> _invoiceReadRepositoryMock;
    private readonly InvoiceListQueryHandler _handler;

    public InvoiceListQueryHandlerTests()
    {
        _invoiceReadRepositoryMock = new Mock<IInvoiceReadRepository>();
        _handler = new InvoiceListQueryHandler(_invoiceReadRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_NullRequest_ShouldThrowApplicationException()
    {
        // Arrange
        InvoiceListRequest request = null!;

        // Act
        Func<Task> act = () => _handler.Handle(request);

        // Assert
        await act.Should()
            .ThrowAsync<ApplicationException>();
    }
}