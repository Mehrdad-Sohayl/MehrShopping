using MehrShopping.Application.Common;
using MehrShopping.Application.Dtos;
using MehrShopping.Application.Services.Invoice.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MehrShopping.Api.Controllers
{
    [ApiController]
    [Route("api/AdminPanel")]
    public class AdminPanelController : ControllerBase
    {
        private readonly InvoiceListQueryHandler _invoiceListQueryHandler;

        public AdminPanelController(InvoiceListQueryHandler invoiceListQueryHandler)
        {
            _invoiceListQueryHandler = invoiceListQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceListAsync([FromQuery] InvoiceListRequest request)
        {
            var result = await _invoiceListQueryHandler.Handle(request);
            return Ok(result);
        }
    }
}
