using FluentValidation;

namespace MeUi.Application.Features.Authentication.Queries.ValidateUserCredentials;

public class ValidateUserCredentialsQueryValidator : AbstractValidator<ValidateUserCredentialsQuery>
{
    public ValidateUserCredentialsQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.LoginMethodCode)
            .NotEmpty()
            .WithMessage("Login method code is required");
    }
}