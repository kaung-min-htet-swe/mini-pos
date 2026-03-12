namespace mini_pos.Features.Products.Dtos;

public record ProductFilter(
    string? SearchTerm,
    Guid? CategoryId,
    int PageNumber = 1,
    int Limit = 10
);