using Microsoft.AspNetCore.Identity;

namespace Movies.Domain.Entities
{
    public class Tenant : IdentityUser
    {
        public string Name { get; set; }
    }
}
