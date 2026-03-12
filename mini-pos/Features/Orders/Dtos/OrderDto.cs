namespace mini_pos.Features.Orders.Dtos;

public record OrderDto(
    Guid Id,
    DateTime OrderDate,
    decimal TotalAmount,
    List<OrderItemDto>? OrderItems = null,
    AdminDto? Merchant = null,
    CustomerDto? Customer = null
);