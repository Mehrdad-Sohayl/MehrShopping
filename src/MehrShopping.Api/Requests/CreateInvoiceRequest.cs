namespace MehrShopping.Api.Requests
{
    public class CreateInvoiceRequest
    {
        public int CustomerId { get; set; }
        public List<CreateInvoiceItemRequest> Items { get; set; }
    }
}
