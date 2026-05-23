namespace MehrShopping.Application.Services.Invoice.Queries;

public class InvoiceListRequest
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 20;
    public IReadOnlyList<InvoiceFilter> Filters { get; init; } = [];
}
