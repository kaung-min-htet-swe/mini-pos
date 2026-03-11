namespace mini_pos.Products.Dtos;

public record CategoryResponseDto(
    Guid Id,
    string Name,
    string? Description
);