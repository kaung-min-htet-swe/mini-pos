using Microsoft.AspNetCore.Mvc;
using mini_pos.Features.Products.Dtos;
using mini_pos.Features.Products.Services;

namespace mini_pos.Products.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ProductFilter filter)
    {
        var result = await productService.List(filter);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await productService.GetById(id);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequestDto productRequest)
    {
        var result = await productService.Create(productRequest);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductRequestDto productRequest)
    {
        var result = await productService.Update(id, productRequest);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await productService.Delete(id);
        return StatusCode(result.StatusCode(), result.Data());
    }
}