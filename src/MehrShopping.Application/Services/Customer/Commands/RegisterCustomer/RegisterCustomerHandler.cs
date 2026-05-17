using MehrShopping.Application.Common;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Application.Interfaces;

namespace MehrShopping.Application.Services.Customer.Commands.RegisterCustomer
{
    public class RegisterCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPersonalInfoClient _personalInfoClient;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerHandler(
            ICustomerRepository customerRepository,
            IPersonalInfoClient personalInfoClient,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _personalInfoClient = personalInfoClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MehrShopping.Domain.Entities.Customer>> Handle(RegisterCustomerCommand command)
        {
            var customer = await _customerRepository.FindByNationalCodeAsync(command.NationalCode);
            if (customer != null)
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerAlreadyExists, nameof(customer)));

            var personalInfo = await _personalInfoClient.GetAsync(command.NationalCode);
            if (personalInfo == null)
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.PersonalInfoNotFound, nameof(personalInfo)));

            var newCustomer = Domain.Entities.Customer.Create(personalInfo.FirstName, personalInfo.LastName, personalInfo.NationalCode);

            await _customerRepository.AddAsync(newCustomer);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Customer>.Success(newCustomer);
        }
    }
}
