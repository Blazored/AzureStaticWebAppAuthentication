using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.AzureStaticWebAppAuthentication.Abstractions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace Blazored.AzureStaticWebAppAuthentication
{
    internal class AzureStaticWebAppAuthenticationStateProvider : AuthenticationStateProvider
    {
        readonly ILogger<AzureStaticWebAppAuthenticationStateProvider> _logger;
        readonly IAzureStaticWebAppUserProvider _userProvider;

        public AzureStaticWebAppAuthenticationStateProvider(
            ILogger<AzureStaticWebAppAuthenticationStateProvider> logger,
            IAzureStaticWebAppUserProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClientPrincipal? client = await _userProvider.GetCurrentUserAsync();

            if (client is null)
                return NotAuthorised();

            var identity = new ClaimsIdentity(client.IdentityProvider);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, client.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, client.UserDetails));
            identity.AddClaims(client.UserRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }



        static AuthenticationState NotAuthorised() => new(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}