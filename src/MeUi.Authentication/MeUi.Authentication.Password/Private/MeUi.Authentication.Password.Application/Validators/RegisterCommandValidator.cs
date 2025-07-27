using System.Data;
using FastEndpoints;
using FluentValidation;
using MeUi.Authentication.Password.ApplicationContract.Commands;

namespace MeUi.Authentication.Password.Application.Validators;

public class RegisterCommandValidator : Validator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(255);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches(@"^[a-zA-Z0-9._]+$")
                .WithMessage("Can only contain letters, numbers, dots, and underscores.")
            .Must(username => !username.Contains("..") && !username.Contains("__") && !username.Contains("._") && !username.Contains("_."))
                .WithMessage("Cannot contain consecutive special characters.")
            .Must(username => !username.StartsWith(".") && !username.StartsWith("_") && !username.EndsWith(".") && !username.EndsWith("_"))
                .WithMessage("Cannot start or end with '.' or '_'.");
    }
}