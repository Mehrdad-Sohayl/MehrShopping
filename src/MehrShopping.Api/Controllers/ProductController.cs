using MehrShopping.Api.Requests;
using MehrShopping.Application.Services.Customer.Commands.RegisterCustomer;
using MehrShopping.Application.Services.Products.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MehrShopping.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly RegisterProductCommandHandler _registerProductCommandHandler;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

        public ProductController(
            RegisterProductCommandHandler registerProductCommandHandler,
            DeleteProductCommandHandler deleteProductCommandHandler)
        {
            _registerProductCommandHandler = registerProductCommandHandler;
            _deleteProductCommandHandler = deleteProductCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterProductRequest request)
        {
            var command = new RegisterProductCommand(request.Name, request.Quantity);

            var result = await _registerProductCommandHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Update(DeleteProductRequest request)
        {
            var command = new DeleteProductCommand(request.Id);

            var result = await _deleteProductCommandHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }
    }
}
