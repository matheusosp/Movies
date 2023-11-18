using FluentValidation;
using Movies.Application.Commands.Tenants;

namespace Movies.Application.Validators.Tenant
{
    public class AuthorizeTenantCommandValidator : AbstractValidator<AuthorizeTenantCommand>
    {
        public AuthorizeTenantCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}