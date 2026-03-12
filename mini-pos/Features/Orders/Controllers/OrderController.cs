using Microsoft.AspNetCore.Mvc;
using mini_pos.Features.Orders.Dtos;
using mini_pos.Features.Orders.Services;

namespace mini_pos.Features.Orders.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] OrderFilter filter)
    {
        var result = await orderService.List(filter);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await orderService.GetById(id);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto order)
    {
        var result = await orderService.Create(order);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrderDto order)
    {
        var result = await orderService.Update(id, order);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await orderService.Delete(id);
        return result.IsSuccess()
            ? StatusCode(result.StatusCode(), new { message = result.Message() })
            : StatusCode(result.StatusCode(), result.Data());
    }
}
