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
        private readonly RegisterProductHandler _registerProductHandler;
        private readonly DeleteProductHandler _deleteProductHandler;

        public ProductController(
            RegisterProductHandler registerProductHandler,
            DeleteProductHandler deleteProductHandler)
        {
            _registerProductHandler = registerProductHandler;
            _deleteProductHandler = deleteProductHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterProductRequest request)
        {
            var command = new RegisterProductCommand(request.Name, request.Quantity);

            var result = await _registerProductHandler.Handle(command);

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

            var result = await _deleteProductHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }
    }
}
