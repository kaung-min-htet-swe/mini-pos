namespace mini_pos.Features.Orders.Dtos;

public record CreateOrderItemDto(
    Guid Id,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal,
    Guid ProductId
);