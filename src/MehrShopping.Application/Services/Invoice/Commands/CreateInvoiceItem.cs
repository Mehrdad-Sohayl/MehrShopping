namespace MehrShopping.Application.Services.Invoice.Commands
{
    // TODO : Bad
    public class CreateInvoiceItem
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }

        public CreateInvoiceItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

    }
}
