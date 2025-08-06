using MediatR;
using MeUi.Application.Features.Authentication.Models;

namespace MeUi.Application.Features.Authentication.Commands.Register;

public record RegisterCommand : IRequest<TokenResponse>
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}