namespace Blazored.AzureStaticWebAppAuthentication.Abstractions
{
    public interface IAzureStaticWebAppNavigationService
    {
        IAzureStaticWebAppNavigator Logout();

        IAzureStaticWebAppLoginProvider Login();
    }
}