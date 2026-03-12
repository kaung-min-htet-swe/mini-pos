namespace mini_pos.Features.Users.Dtos;

public record MerchantDto(
    Guid Id,
    string Username,
    string Email,
    string Role
);