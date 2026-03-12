using Microsoft.AspNetCore.Mvc;
using mini_pos.Features.Products.Dtos;
using mini_pos.Features.Products.Services;

namespace mini_pos.Products.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] CategoryFilter filter)
    {
        var result = await categoryService.List(filter);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await categoryService.GetById(id);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDto categoryRequestDto)
    {
        var result = await categoryService.Create(categoryRequestDto);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryRequestDto categoryRequestDto)
    {
        var result = await categoryService.Update(id, categoryRequestDto);
        return StatusCode(result.StatusCode(), result.Data());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await categoryService.Delete(id);
        return StatusCode(result.StatusCode(), result.Data());
    }
}