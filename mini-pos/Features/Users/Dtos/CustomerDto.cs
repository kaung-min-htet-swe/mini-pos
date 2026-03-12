namespace mini_pos.Features.Users.Dtos;

public record CustomerDto(
    Guid Id,
    string Name,
    string? PhoneNumber,
    string? Email
);