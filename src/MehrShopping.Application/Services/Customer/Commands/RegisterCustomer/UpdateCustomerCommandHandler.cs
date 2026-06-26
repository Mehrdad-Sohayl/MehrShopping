using MehrShopping.Application.Common;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MehrShopping.Application.Services.Customer.Commands.RegisterCustomer
{
    public class UpdateCustomerCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPersonalInfoClient _personalInfoClient;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IPersonalInfoClient personalInfoClient,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _personalInfoClient = personalInfoClient;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Domain.Entities.Customer>> Handle(RegisterCustomerCommand command, CancellationToken cancellationToken)
        {
            if (command is null) 
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.RequestValidation, nameof(command)));

            var customer = await _customerRepository.FindByNationalCodeAsync(command.NationalCode);
            if (customer == null)
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerAlreadyExists, nameof(customer)));

            var personalInfo = await _personalInfoClient.GetAsync(command.NationalCode, cancellationToken);

            if (personalInfo == null)
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.PersonalInfoNotFound, nameof(personalInfo)));

            customer.Update(personalInfo.FirstName, personalInfo.LastName);

            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Customer>.Success(customer);
        }
    }
}