namespace mini_pos.Features.Orders.Dtos;

public record OrderFilter(
    int PageNumber = 1,
    int Limit = 10
);