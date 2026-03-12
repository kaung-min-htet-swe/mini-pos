namespace mini_pos.Features.Orders.Dtos;

public record CreateOrderDto(
    Guid CustomerId,
    Guid MerchantId,
    DateTime OrderDate,
    decimal TotalAmount,
    List<CreateOrderItemDto> OrderItems
);