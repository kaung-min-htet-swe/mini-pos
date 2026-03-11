namespace mini_pos.Products.Dtos;

public record CreateProductRequestDto(
    string Name,
    decimal Price,
    string Sku,
    int StockQuantity,
    string CategoryId
);