using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Movies.Domain.Entities
{
    public class Tenant : IdentityUser
    {
        public string Name { get; set; }
    }
}
