namespace MehrShopping.Application.Services.Invoice.Commands
{
    public class CreateInvoiceItem
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
