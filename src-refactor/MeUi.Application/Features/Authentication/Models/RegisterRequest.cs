namespace MeUi.Application.Features.Authentication.Models;

public record RegisterRequest
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}