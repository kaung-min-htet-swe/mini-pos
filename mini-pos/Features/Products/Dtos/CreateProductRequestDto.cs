namespace mini_pos.Features.Products.Dtos;

public record CreateProductRequestDto(
    string Name,
    decimal Price,
    string Sku,
    int StockQuantity,
    string CategoryId
);