namespace mini_pos.Features.Users.Dtos;

public record CustomerCreateDto(
    string Name,
    string? PhoneNumber,
    string? Email
);