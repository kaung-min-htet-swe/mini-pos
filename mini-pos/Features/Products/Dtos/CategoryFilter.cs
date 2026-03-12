namespace mini_pos.Features.Products.Dtos;

public record CategoryFilter(
    string? SearchTerm,
    int PageNumber = 1,
    int Limit = 10
);