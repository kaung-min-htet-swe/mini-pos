namespace mini_pos.Features.Orders.Dtos;

public record OrderItemDto(
    Guid Id,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal,
    ProductDto Product
);