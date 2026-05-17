namespace MehrShopping.Application.Services.Invoice.Commands
{
    public class CreateInvoiceCommand
    {
        public CreateInvoiceCommand(int customerId, List<CreateInvoiceItem> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public int CustomerId { get; init; }
        public List<CreateInvoiceItem> Items { get; init; }
    }
}
