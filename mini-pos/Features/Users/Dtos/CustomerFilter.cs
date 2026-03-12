namespace mini_pos.Features.Users.Dtos;

public record CustomerFilter(
    string? SearchTerm,
    int PageNumber = 1,
    int Limit = 10
);