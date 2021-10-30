using System.Threading.Tasks;

namespace Blazored.AzureStaticWebAppAuthentication.Abstractions
{
    public interface IAzureStaticWebAppUserProvider
    {
        Task<ClientPrincipal?> GetCurrentUserAsync();
    }
}