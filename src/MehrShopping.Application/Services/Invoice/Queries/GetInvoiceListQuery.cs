namespace MehrShopping.Application.Services.Invoice.Queries
{
    public class GetInvoiceListQuery
    {
        public InvoiceListRequest Request { get; }

        public GetInvoiceListQuery(InvoiceListRequest request)
        {
            Request = request;
        }
    }
}
