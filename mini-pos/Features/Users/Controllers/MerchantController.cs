using Microsoft.AspNetCore.Mvc;
using mini_pos.Features.Users.Dtos;
using mini_pos.Features.Users.Services;

namespace mini_pos.Features.Users.Controllers;

[ApiController]
[Route("api/merchants")]
public class MerchantController(IMerchantService merchantService):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] MerchantFilter filter)
    {
        var result = await merchantService.List(filter);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await merchantService.GetById(id);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MerchantCreateDto merchant)
    {
        var result = await merchantService.Create(merchant);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MerchantCreateDto merchant)
    {
        var result = await merchantService.Update(id, merchant);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await merchantService.Delete(id);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }
}