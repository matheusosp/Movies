using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Entities.Enums;

namespace Movies.CrossCutting.IdentityErrors
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber, ICustomIdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = BusinessErrors.EmailAlreadyRegistered.ToString()
            };
        }
    }

}