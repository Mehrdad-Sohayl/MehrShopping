using MehrShopping.Api.Requests;
using MehrShopping.Application.Services.Customer.Commands.RegisterCustomer;
using MehrShopping.Application.Services.Invoice.Commands;
using MehrShopping.Application.Services.Products.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MehrShopping.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly CreateInvoiceHandler _createInvoiceHandler;

        public InvoiceController(CreateInvoiceHandler createInvoiceHandler)
        {
            _createInvoiceHandler = createInvoiceHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceRequest request)
        {
            var items = request.Items.Select(i => new CreateInvoiceItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
            }).ToList();

            var command = new CreateInvoiceCommand(request.CustomerId, items);

            var result = await _createInvoiceHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }
    }
}
