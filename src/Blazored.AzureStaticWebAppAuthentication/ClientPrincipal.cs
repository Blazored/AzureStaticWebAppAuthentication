using System.Collections.Generic;

namespace Blazored.AzureStaticWebAppAuthentication
{
    public class ClientPrincipal
    {
        public string? IdentityProvider { get; set; }

        public string? UserId { get; set; }

        public string? UserDetails { get; set; }

        public List<string> UserRoles { get; set; } = new();
    }
}