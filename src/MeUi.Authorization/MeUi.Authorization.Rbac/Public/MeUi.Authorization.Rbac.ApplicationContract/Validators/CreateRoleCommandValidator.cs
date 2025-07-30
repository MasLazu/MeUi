using FastEndpoints;
using FluentValidation;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Validators;

public class CreateRoleCommandValidator : Validator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(255);
    }
}