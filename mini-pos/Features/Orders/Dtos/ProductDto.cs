namespace mini_pos.Features.Orders.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    string Sku
);