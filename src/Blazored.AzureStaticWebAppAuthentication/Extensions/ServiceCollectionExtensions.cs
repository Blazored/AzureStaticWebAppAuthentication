using Blazored.AzureStaticWebAppAuthentication.Abstractions;
using Blazored.AzureStaticWebAppAuthentication.Internals;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Blazored.AzureStaticWebAppAuthentication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorAzureStaticWebApp(this IServiceCollection services) =>
            services
                .AddScoped<IAzureStaticWebAppNavigationService, AzureStaticWebAppNavigationService>()
                .AddScoped<AuthenticationStateProvider, AzureStaticWebAppAuthenticationStateProvider>()
                .AddScoped<IAzureStaticWebAppUserProvider, AzureStaticWebAppUserProvider>();
    }
}