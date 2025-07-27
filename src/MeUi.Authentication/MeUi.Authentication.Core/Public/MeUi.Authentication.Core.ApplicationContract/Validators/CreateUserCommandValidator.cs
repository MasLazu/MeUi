using FastEndpoints;
using FluentValidation;
using MeUi.Authentication.Core.ApplicationContract.Commands;

namespace MeUi.Authentication.Core.ApplicationContract.Validators;

public class CreateUserCommandValidator : Validator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Email)
            .MaximumLength(255);

        RuleFor(x => x.Username)
            .MaximumLength(255);
    }
}