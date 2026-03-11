namespace mini_pos.Products.Dtos;

public record ProductResponseDto(
    Guid ProductId,
    string Name,
    Decimal Price,
    string Sku,
    int StockQuantity
);