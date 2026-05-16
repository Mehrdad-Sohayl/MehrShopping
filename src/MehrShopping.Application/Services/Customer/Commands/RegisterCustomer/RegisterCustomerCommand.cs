namespace MehrShopping.Application.Services.Customer.Commands.RegisterCustomer
{
    public class RegisterCustomerCommand
    {
        public string NationalCode { get; init; }

        public RegisterCustomerCommand(string nationalCode)
        {
            NationalCode = nationalCode;
        }
    }

    public class UpdateCustomerCommand
    {
        public string NationalCode { get; init; }

        public UpdateCustomerCommand(string nationalCode)
        {
            NationalCode = nationalCode;
        }
    }
}
