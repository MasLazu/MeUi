using FastEndpoints;
using FluentValidation;
using MeUi.Authorization.Rbac.ApplicationContract.Commands;

namespace MeUi.Authorization.Rbac.ApplicationContract.Validators;

public class UpdateRoleCommandValidator : Validator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .MaximumLength(255);
    }
}