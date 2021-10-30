using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.AzureStaticWebAppAuthentication.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blazored.AzureStaticWebAppAuthentication.Internals
{
    class AzureStaticWebAppUserProvider : IAzureStaticWebAppUserProvider
    {
        readonly HttpClient _httpClient;
        readonly ILogger<AzureStaticWebAppUserProvider> _logger;

        public AzureStaticWebAppUserProvider(HttpClient httpClient, ILogger<AzureStaticWebAppUserProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public async Task<ClientPrincipal?> GetCurrentUserAsync()
        {
            HttpResponseMessage? response = await _httpClient.GetAsync(AzureStaticWebApp.Auth.Me);

            if (response.IsSuccessStatusCode)
                return await GetClientPrincipal(response);
            
            _logger.LogError($"Unable to get user's information with status code {response.StatusCode}");

            return null;
        }
        
        async Task<ClientPrincipal?> GetClientPrincipal(HttpResponseMessage response)
        {
            try
            {
                UserResponse? userResponse = JsonSerializer.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync(),
                    JsonSettings.Options);
                return userResponse.ClientPrincipal;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to extract client principal from response.");
            }

            
            return null;
        }
    }
}