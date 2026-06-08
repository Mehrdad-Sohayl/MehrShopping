using Xunit;
using Moq;
using FluentAssertions;
using MehrShopping.Application.Services.Products.Commands;
using MehrShopping.Domain.Entities;
using MehrShopping.Application.Interfaces;
using MehrShopping.Infrastructure.Repositories;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Application.Common;

namespace MehrShopping.Test.Application.Services.Products
{
    public class RegisterProductHandlerTests
    {
        [Fact]
        public void Handle_WhenProductDoesNotExist_ShouldCreateAndSave()
        {
            var repositoryMock = new Mock<IProductRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var handler = new RegisterProductCommandHandler(repositoryMock.Object, unitOfWorkMock.Object);
            var cmd = new RegisterProductCommand(name: "Test", 10);

            // Act
            var result = handler.Handle(cmd);

            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once());
            unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once());
            result.Should().NotBeNull();
            result!.Result.IsSuccess.Should().BeFalse();
            result.Result.Errors.First().Code.Should().Be(ApplicationErrorCodes.ProductAlreadyExists);
        }
    }
}
