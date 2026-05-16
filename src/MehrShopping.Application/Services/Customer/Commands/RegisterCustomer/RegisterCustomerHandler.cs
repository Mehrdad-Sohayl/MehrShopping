using MehrShopping.Application.Common;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Application.Interfaces;

namespace MehrShopping.Application.Services.Customer.Commands.RegisterCustomer
{
    public class RegisterCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPersonalInfoService _personalInfoService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerHandler(
            ICustomerRepository customerRepository,
            IPersonalInfoService personalInfoService,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _personalInfoService = personalInfoService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MehrShopping.Domain.Entities.Customer>> Handle(RegisterCustomerCommand command)
        {
            var customer = await _customerRepository.FindByNationalCodeAsync(command.NationalCode);
            if (customer != null)
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerAlreadyExists, nameof(customer)));

            var personalInfo = await _personalInfoService.GetAsync(command.NationalCode);

            var newCustomer = Domain.Entities.Customer.Create(personalInfo.FirstName, personalInfo.LastName, personalInfo.NationalCode);

            await _customerRepository.AddAsync(newCustomer);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Customer>.Success(newCustomer);
        }
    }

    public class UpdateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPersonalInfoService _personalInfoService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerHandler(
            ICustomerRepository customerRepository,
            IPersonalInfoService personalInfoService,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _personalInfoService = personalInfoService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MehrShopping.Domain.Entities.Customer>> Handle(RegisterCustomerCommand command)
        {
            var customer = await _customerRepository.FindByNationalCodeAsync(command.NationalCode);
            if (customer == null)
                return Result<Domain.Entities.Customer>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerAlreadyExists, nameof(customer)));

            var personalInfo = await _personalInfoService.GetAsync(command.NationalCode);

            customer.Update(personalInfo.FirstName, personalInfo.LastName);

            _customerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Customer>.Success(customer);
        }
    }
}
