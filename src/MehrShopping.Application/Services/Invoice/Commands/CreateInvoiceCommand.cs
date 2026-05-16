using MehrShopping.Domain.Entities;

namespace MehrShopping.Application.Services.Invoice.Commands
{
    public class CreateInvoiceCommand
    {
        public int CustomerId { get; init; }
        public List<InvoiceItem> Items { get; init; }
    }
}
