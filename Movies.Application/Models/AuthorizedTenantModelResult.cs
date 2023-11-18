using System;

namespace Movies.Application.Models
{
    public class AuthorizedTenantModelResult
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}