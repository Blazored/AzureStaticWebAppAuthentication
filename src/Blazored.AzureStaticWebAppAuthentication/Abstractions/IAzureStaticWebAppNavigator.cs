namespace Blazored.AzureStaticWebAppAuthentication.Abstractions
{
    public interface IAzureStaticWebAppNavigator
    {
        void Navigate();

        IAzureStaticWebAppNavigator WithRedirect(string redirectUri);
    }
}