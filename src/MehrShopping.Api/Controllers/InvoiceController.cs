using MehrShopping.Api.Requests;
using MehrShopping.Application.Services.Invoice.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MehrShopping.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly CreateInvoiceCommandHandler _createInvoiceCommandHandler;

        public InvoiceController(CreateInvoiceCommandHandler createInvoiceCommandHandler)
        {
            _createInvoiceCommandHandler = createInvoiceCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceRequest request)
        {
            var items = request.Items.Select(i => new CreateInvoiceItem(
                productId: i.ProductId,
                quantity: i.Quantity)).ToList();

            var command = new CreateInvoiceCommand(request.CustomerId, items);

            var result = await _createInvoiceCommandHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }
    }
}
