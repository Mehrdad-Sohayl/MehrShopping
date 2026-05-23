using MehrShopping.Application.Common;
using MehrShopping.Application.Dtos;
using MehrShopping.Application.Interfaces;
using ApplicationException = MehrShopping.Application.Exceptions.ApplicationException;

namespace MehrShopping.Application.Services.Invoice.Queries
{
    public class InvoiceListQueryHandler
    {
        private readonly IInvoiceReadRepository _invoiceReadRepository;

        public InvoiceListQueryHandler(IInvoiceReadRepository invoiceReadRepository)
        {
            _invoiceReadRepository = invoiceReadRepository;
        }

        public async Task<Result<List<InvoiceListDto>>> Handle(InvoiceListRequest request)
        {

            if (request == null)
                throw new ApplicationException(new ApplicationError(ApplicationErrorCodes.RequestValidation, nameof(InvoiceListRequest)));

            var result = await _invoiceReadRepository.GetInvoiceListAsync(request);
            return result;
        }
    }
}
