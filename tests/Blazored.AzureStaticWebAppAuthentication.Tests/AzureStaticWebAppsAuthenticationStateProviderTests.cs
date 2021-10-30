using Blazored.AzureStaticWebAppAuthentication.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Blazored.AzureStaticWebAppAuthentication.Tests
{
    public class AzureStaticWebAppsAuthenticationStateProviderTests
    {
        readonly AzureStaticWebAppAuthenticationStateProvider _authenticationStateProvider;

        readonly MockHttpMessageHandler _httpMessageHandler = new();
        readonly Mock<IAzureStaticWebAppUserProvider> _userProvider = new();

        public AzureStaticWebAppsAuthenticationStateProviderTests() => _authenticationStateProvider = new AzureStaticWebAppAuthenticationStateProvider(
                new NullLogger<AzureStaticWebAppAuthenticationStateProvider>(), _userProvider.Object);

        [Fact]
        public async Task GetAuthenticationStateGivenNoUserReturnsUnAuthorized()
        {
            //Arrange
            //Act
            AuthenticationState? authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            //Act
            authenticationState.User!.Identity!.IsAuthenticated.Should().BeFalse();
        }

        [Fact]
        public async Task
            GetAuthenticationStateGivenUserReturnsCorrectAuthorizedState()
        {
            //Arrange
            var principal = new ClientPrincipal
            {
                IdentityProvider = "github",
                UserId = Guid.NewGuid().ToString(),
                UserDetails = "joe bloggs",
                UserRoles = new List<string> { "anonymous", "authorized" }
            };

            _userProvider.Setup(o => o.GetCurrentUserAsync()).ReturnsAsync(principal);

            //Act   
            AuthenticationState? authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            //Act
            authenticationState.User!.Identity!.IsAuthenticated.Should().BeTrue();
            authenticationState.User.Identity.AuthenticationType.Should().Be(principal.IdentityProvider);
            authenticationState.User.Claims.Count(c => c.Type == ClaimTypes.Role).Should().Be(2);
            authenticationState.User.IsInRole("anonymous").Should().BeTrue();
            authenticationState.User.IsInRole("authorized").Should().BeTrue();
            authenticationState.User.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "joe bloggs");
            authenticationState.User.Claims.Should()
                .Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == principal.UserId);
        }
    }
}