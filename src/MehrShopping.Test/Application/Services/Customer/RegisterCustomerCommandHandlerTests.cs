using FluentAssertions;
using MehrShopping.Application.Common;
using MehrShopping.Application.Interfaces;
using MehrShopping.Application.Services.Customer.Commands.RegisterCustomer;
using MehrShopping.Domain.Interfaces.Repositories;
using Moq;
using CustomerEntity = MehrShopping.Domain.Entities.Customer;

namespace MehrShopping.Test.Application.Services.Customer
{
    public class RegisterCustomerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenNationalCodeUnique_ShouldCreate()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var personalInfoClientMock = new Mock<IPersonalInfoClient>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            customerRepositoryMock
                .Setup(c => c.FindByNationalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerEntity?)null);

            var personalInfo = new PersonalInfoDto(FirstName: "First", LastName: "Last", NationalCode: "1234567890");

            personalInfoClientMock
                .Setup(p => p.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(personalInfo);

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync());

            var handler = new RegisterCustomerCommandHandler(customerRepositoryMock.Object, personalInfoClientMock.Object, unitOfWorkMock.Object);
            var cmd = new RegisterCustomerCommand(personalInfo.NationalCode);

            var result = await handler.Handle(cmd, CancellationToken.None);

            customerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>()), Times.Once());
            personalInfoClientMock.Verify(p => p.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Once());
            unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once());

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WhenNationalCodeNotUnique_ShouldThrow()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var personalInfoClientMock = new Mock<IPersonalInfoClient>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var customerEntity = CustomerEntity.Create("First", "Last", "0123456789");
            customerRepositoryMock
                .Setup(c => c.FindByNationalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(customerEntity);

            var personalInfo = new PersonalInfoDto(FirstName: "First", LastName: "Last", NationalCode: "1234567890");

            personalInfoClientMock
                .Setup(p => p.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(personalInfo);

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync());

            var handler = new RegisterCustomerCommandHandler(customerRepositoryMock.Object, personalInfoClientMock.Object, unitOfWorkMock.Object);
            var cmd = new RegisterCustomerCommand(personalInfo.NationalCode);

            var result = await handler.Handle(cmd, CancellationToken.None);

            personalInfoClientMock.Verify(p => p.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Never);
            customerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<CustomerEntity>()), Times.Never);
            unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Never);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Errors.First().Code.Should().Be(ApplicationErrorCodes.CustomerAlreadyExists);
        }

        [Fact]
        public async Task Handle_WhenNationalCodeNotFoundInPersonalInfo_ShouldThrow()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var personalInfoClientMock = new Mock<IPersonalInfoClient>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var customerEntity = CustomerEntity.Create("First", "Last", "0123456789");
            customerRepositoryMock
                .Setup(c => c.FindByNationalCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerEntity)null);

            personalInfoClientMock
                .Setup(p => p.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((PersonalInfoDto)null);

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync());

            var handler = new RegisterCustomerCommandHandler(customerRepositoryMock.Object, personalInfoClientMock.Object, unitOfWorkMock.Object);
            var cmd = new RegisterCustomerCommand(customerEntity.NationalCode.Value);

            var result = await handler.Handle(cmd, CancellationToken.None);

            personalInfoClientMock.Verify(p => p.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
            customerRepositoryMock.Verify(c => c.FindByNationalCodeAsync(It.IsAny<string>()), Times.Once);
            customerRepositoryMock.Verify(c => c.AddAsync(It.IsAny<CustomerEntity>()), Times.Never);
            unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Never);

            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.Errors.First().Code.Should().Be(ApplicationErrorCodes.PersonalInfoNotFound);
        }
    }
}
