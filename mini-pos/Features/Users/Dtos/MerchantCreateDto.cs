namespace mini_pos.Features.Users.Dtos;

public record MerchantCreateDto(
    string Username,
    string Email,
    string Password
);