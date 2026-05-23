using MehrShopping.Application.Common;
using MehrShopping.Application.Dtos;
using MehrShopping.Application.Services.Invoice.Queries;

namespace MehrShopping.Application.Interfaces
{
    public interface IInvoiceReadRepository
    {
        Task<PagedResult<List<InvoiceListDto>>> GetInvoiceListAsync(InvoiceListRequest request);
    }
}
