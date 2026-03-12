namespace mini_pos.Features.Products.Dtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    decimal Price,
    string Sku,
    int StockQuantity
);