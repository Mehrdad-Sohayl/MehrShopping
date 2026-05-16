using MehrShopping.Api.Requests;
using MehrShopping.Application.Services.Customer.Commands.RegisterCustomer;
using Microsoft.AspNetCore.Mvc;

namespace MehrShopping.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly RegisterCustomerHandler _registerCustomerHandler;
        private readonly UpdateCustomerHandler _updateCustomerHandler;

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCustomerRequest request)
        {
            var command = new RegisterCustomerCommand(request.NationalCode);

            var result = await _registerCustomerHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerRequest request)
        {
            var command = new RegisterCustomerCommand(request.NationalCode);

            var result = await _updateCustomerHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }
    }
}
