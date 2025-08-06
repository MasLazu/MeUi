using FluentValidation;

namespace MeUi.Application.Features.Authentication.Queries.CanUserAuthenticate;

public class CanUserAuthenticateQueryValidator : AbstractValidator<CanUserAuthenticateQuery>
{
    public CanUserAuthenticateQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.LoginMethodCode)
            .NotEmpty()
            .WithMessage("Login method code is required");
    }
}