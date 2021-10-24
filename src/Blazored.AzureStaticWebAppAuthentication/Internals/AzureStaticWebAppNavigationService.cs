using System.Text;
using Blazored.AzureStaticWebAppAuthentication.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Blazored.AzureStaticWebAppAuthentication.Internals
{
    internal class AzureStaticWebAppNavigationService : IAzureStaticWebAppNavigationService,
        IAzureStaticWebAppNavigator, IAzureStaticWebAppLoginProvider
    {
        readonly StringBuilder _loginBuilder = new();
        readonly NavigationManager _navigationManager;

        public AzureStaticWebAppNavigationService(NavigationManager navigationManager) => 
            _navigationManager = navigationManager;

        public IAzureStaticWebAppNavigator WithGithub()
        {
            _loginBuilder.Append("github");
            return this;
        }

        public IAzureStaticWebAppNavigator WithTwitter()
        {
            _loginBuilder.Append("twitter");
            return this;
        }

        public IAzureStaticWebAppNavigator WithAzureActiveDirectory()
        {
            _loginBuilder.Append("aad");
            return this;
        }

        public IAzureStaticWebAppNavigator WithRedirect(string redirectUri)
        {
            _loginBuilder.Append($"?post_login_redirect_uri={redirectUri}");
            return this;
        }

        public IAzureStaticWebAppNavigator Logout()
        {
            _loginBuilder.Append("/.auth/logout");
            return this;
        }

        public IAzureStaticWebAppLoginProvider Login()
        {
            _loginBuilder.Append("/.auth/login/");
            return this;
        }

        public void Navigate()
        {
            _navigationManager.NavigateTo(_loginBuilder.ToString(), true);
            _loginBuilder.Clear();
        }

        public override string ToString() => _loginBuilder.ToString();
    }
}