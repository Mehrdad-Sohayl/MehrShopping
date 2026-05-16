namespace MehrShopping.Application.Services.Invoice.Commands
{
    public class CreateInvoiceCommand
    {
        public int CustomerId { get; init; }
        public List<CreateInvoiceItem> Items { get; init; }
    }
}
