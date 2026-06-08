using Xunit;
using Moq;
using FluentAssertions;
using MehrShopping.Application.Services.Products.Commands;
using MehrShopping.Domain.Entities;
using MehrShopping.Application.Interfaces;
using MehrShopping.Infrastructure.Repositories;
using MehrShopping.Domain.Interfaces.Repositories;

namespace MehrShopping.Test.Application.Services.Products
{
    public class DeleteProductHandlerTests
    {
        [Fact]
        public async Task Handle_WhenProductExists_ShouldDeleteAndSave()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            productRepositoryMock
                .Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<Product>());

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync());

            var cmd = new DeleteProductCommand(1);

            var handler = new DeleteProductCommandHandler(productRepositoryMock.Object, unitOfWorkMock.Object);

            var result = await handler.Handle(cmd);

            productRepositoryMock
                .Verify(p => p.GetByIdAsync(It.IsAny<int>()), Times.Once);

            unitOfWorkMock
                .Verify(u => u.SaveChangesAsync(), Times.Once);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
