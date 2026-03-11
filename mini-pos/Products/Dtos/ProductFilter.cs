namespace mini_pos.Products.Dtos;

public record ProductFilter(
    string? SearchTerm,
    Guid? CategoryId,
    int PageNumber = 1,
    int Limit = 10
);