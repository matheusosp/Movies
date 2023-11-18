using MediatR;
using Movies.Application.Models;
using Movies.Domain.Generic;

namespace Movies.Application.Commands.Tenants
{
    public class AuthorizeTenantCommand : IRequest<IGenericCommandResult<AuthorizedTenantModelResult>>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}