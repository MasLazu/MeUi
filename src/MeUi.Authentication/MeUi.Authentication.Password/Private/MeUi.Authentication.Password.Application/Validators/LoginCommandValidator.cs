using FastEndpoints;
using FluentValidation;
using MeUi.Authentication.Password.ApplicationContract.Commands;

namespace MeUi.Authentication.Password.Application.Validators;

public class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(255);
    }
}