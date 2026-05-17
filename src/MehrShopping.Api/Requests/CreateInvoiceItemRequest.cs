namespace MehrShopping.Api.Requests
{
    public class CreateInvoiceItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
