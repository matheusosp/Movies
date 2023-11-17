using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.CrossCutting.IdentityErrors
{
    public interface ICustomIdentityErrorDescriber
    {
        IdentityError DuplicateEmail(string email);
    }
}
