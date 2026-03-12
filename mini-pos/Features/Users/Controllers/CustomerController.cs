using Microsoft.AspNetCore.Mvc;
using mini_pos.Features.Users.Dtos;
using mini_pos.Features.Users.Services;

namespace mini_pos.Features.Users.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] CustomerFilter filter)
    {
        var result = await customerService.List(filter);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await customerService.GetById(id);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerCreateDto customer)
    {
        var result = await customerService.Create(customer);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CustomerCreateDto customer)
    {
        var result = await customerService.Update(id, customer);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await customerService.Delete(id);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }
}