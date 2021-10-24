using Blazored.AzureStaticWebAppAuthentication.Internals;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Moq;
using Xunit;

namespace Blazored.AzureStaticWebAppAuthentication.Tests
{
    public class AzureStaticWebAppNavigationServiceTests
    {
        readonly Mock<NavigationManager> _navigationManager = new();
        readonly AzureStaticWebAppNavigationService _navigationService;

        public AzureStaticWebAppNavigationServiceTests() => _navigationService = new AzureStaticWebAppNavigationService(_navigationManager.Object);

        [Fact]
        public void LoginWithGithubNavigatesCorrectly() => 
            _navigationService.Login().WithGithub().ToString().Should().Be("/.auth/login/github");

        [Fact]
        public void LoginWithTwitterNavigatesCorrectly() =>
            _navigationService.Login().WithTwitter().ToString().Should().Be("/.auth/login/twitter");

        [Fact]
        public void LoginWithMicrosoftNavigatesCorrectly() =>
            _navigationService.Login().WithAzureActiveDirectory().ToString().Should().Be("/.auth/login/aad");

        [Fact]
        public void LoginWithMicrosoftAndRedirectNavigatesCorrectly() =>
            _navigationService.Login().WithAzureActiveDirectory().WithRedirect("http://test.com").ToString().Should().Be("/.auth/login/aad?post_login_redirect_uri=http://test.com");
    }
}
