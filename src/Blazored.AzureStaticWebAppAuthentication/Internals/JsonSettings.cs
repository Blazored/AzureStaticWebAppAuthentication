using System.Text.Json;

namespace Blazored.AzureStaticWebAppAuthentication.Internals
{
    internal static class JsonSettings
    {
        internal static JsonSerializerOptions Options => new() { PropertyNameCaseInsensitive = true };
    }
}