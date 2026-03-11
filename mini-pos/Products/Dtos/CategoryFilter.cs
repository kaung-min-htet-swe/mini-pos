namespace mini_pos.Products.Dtos;

public record CategoryFilter(
    string? SearchTerm,
    int PageNumber = 1,
    int Limit = 10
);