namespace mini_pos.Products.Dtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    Decimal Price,
    string Sku,
    int StockQuantity
);