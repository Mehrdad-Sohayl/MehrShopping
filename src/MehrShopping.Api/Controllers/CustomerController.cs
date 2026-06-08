using MehrShopping.Api.Requests;
using MehrShopping.Application.Services.Customer.Commands.RegisterCustomer;
using Microsoft.AspNetCore.Mvc;

namespace MehrShopping.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly RegisterCustomerCommandHandler _registerCustomerCommandHandler;
        private readonly UpdateCustomerCommandHandler _updateCustomerCommandHandler;

        public CustomerController(
            RegisterCustomerCommandHandler registerCustomerCommandHandler,
            UpdateCustomerCommandHandler updateCustomerCommandHandler)
        {
            _registerCustomerCommandHandler = registerCustomerCommandHandler;
            _updateCustomerCommandHandler = updateCustomerCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCustomerRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCustomerCommand(request.NationalCode);

            var result = await _registerCustomerCommandHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCustomerCommand(request.NationalCode);

            var result = await _updateCustomerCommandHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            if (result != null && result.Errors.Any())
                return BadRequest(result.Errors.FirstOrDefault()!.Message);

            return BadRequest();
        }
    }
}
