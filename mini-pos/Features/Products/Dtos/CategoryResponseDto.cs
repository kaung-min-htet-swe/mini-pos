namespace mini_pos.Features.Products.Dtos;

public record CategoryResponseDto(
    Guid Id,
    string Name,
    string? Description,
    List<ProductResponseDto>? Products = null
);