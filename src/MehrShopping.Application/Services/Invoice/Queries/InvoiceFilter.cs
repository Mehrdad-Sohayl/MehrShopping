namespace MehrShopping.Application.Services.Invoice.Queries
{
    public class InvoiceFilter
    {
        public InvoiceFilterField Field { get; init; }
        public FilterOperator Operator { get; init; }
        public string Value { get; init; } = string.Empty;
    }
}
