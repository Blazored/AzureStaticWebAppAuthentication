namespace Blazored.AzureStaticWebAppAuthentication.Abstractions
{
    public interface IAzureStaticWebAppLoginProvider
    {
        IAzureStaticWebAppNavigator WithGithub();

        IAzureStaticWebAppNavigator WithTwitter();

        IAzureStaticWebAppNavigator WithAzureActiveDirectory();
    }
}