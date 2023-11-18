using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movies.Application.Commands.Tenants;
using Movies.Application.Models;
using Movies.Domain.Entities;
using Movies.Domain.Entities.Enums;
using Movies.Domain.Generic;

namespace Movies.Application.CommandHandlers.Tenants
{
    public class AuthorizeTenantCommandHandler: IRequestHandler<AuthorizeTenantCommand,
        IGenericCommandResult<AuthorizedTenantModelResult>>
    {
        private readonly UserManager<Tenant> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IGenericCommandResult<AuthorizedTenantModelResult> _result;
        private readonly SignInManager<Tenant> _signInManager;

        public AuthorizeTenantCommandHandler(SignInManager<Tenant> signInManager,
            IConfiguration configuration, IGenericCommandResult<AuthorizedTenantModelResult> result,
            UserManager<Tenant> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _result = result;
        }

        public async Task<IGenericCommandResult<AuthorizedTenantModelResult>> Handle(AuthorizeTenantCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return _result.Fail(BusinessErrors.EmailNotFound.ToString());

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded is false)
                return _result.Fail(BusinessErrors.PasswordIncorrect.ToString());

            var roles = await _userManager.GetRolesAsync(user);
            return _result.Ok(GenerateToken(user, roles.FirstOrDefault()));
        }

        private AuthorizedTenantModelResult GenerateToken(Domain.Entities.Tenant userInfo, string role)
        {
            var claims = new List<Claim>();
            if (userInfo.Name != null) claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Role, role));
            
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            
            var expirationConfiguration = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expirationConfiguration));
            
            var token = new JwtSecurityToken(
                _configuration["TokenConfiguration:Issuer"],
                _configuration["TokenConfiguration:Audience"],
                claims,
                expires: expiration,
                signingCredentials: signinCredentials);
            
            return new AuthorizedTenantModelResult
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}