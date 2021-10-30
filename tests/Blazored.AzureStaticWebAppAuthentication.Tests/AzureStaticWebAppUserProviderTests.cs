using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.AzureStaticWebAppAuthentication.Abstractions;
using Blazored.AzureStaticWebAppAuthentication.Internals;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Blazored.AzureStaticWebAppAuthentication.Tests
{
    public class AzureStaticWebAppUserProviderTests
    {
        readonly MockHttpMessageHandler _httpMessageHandler = new();    
        readonly string _authMeUrl = $"http://test.com/{AzureStaticWebApp.Auth.Me}";
        readonly IAzureStaticWebAppUserProvider _userProvider;

        public AzureStaticWebAppUserProviderTests()
        {
            var client = _httpMessageHandler.ToHttpClient();
            client.BaseAddress = new Uri("http://test.com/");
            _userProvider = new AzureStaticWebAppUserProvider(client, new NullLogger<AzureStaticWebAppUserProvider>());
        }
        
        [Fact]
        public async Task GetCurrentUserAsyncNoUserReturnsNull()
        {
            //Arrange
            _httpMessageHandler
                .When(HttpMethod.Get, _authMeUrl)
                .Respond(HttpStatusCode.BadRequest);

            //Act
            ClientPrincipal? user = await _userProvider.GetCurrentUserAsync();

            //Assert
            user.Should().BeNull();
        }

        [Fact]
        public async Task GetCurrentUserAsyncInvalidJsonReturnsNull()
        {
            //Arrange
            _httpMessageHandler
                .When(HttpMethod.Get, _authMeUrl)
                .Respond("application/json", "");

            //Act
            ClientPrincipal? user = await _userProvider.GetCurrentUserAsync();

            //Assert
            user.Should().BeNull();
        }
        
        [Fact]
        public async Task GetCurrentUserAsyncUserDataReturnsClientPrincipal()
        {
            //Arrange
            var principal = new ClientPrincipal
            {
                IdentityProvider = "github",
                UserId = Guid.NewGuid().ToString(),
                UserDetails = "joe bloggs",
                UserRoles = new List<string> { "anonymous", "authorized" }
            };

            string json = JsonSerializer.Serialize(new UserResponse { ClientPrincipal = principal }, JsonSettings.Options);

            _httpMessageHandler
                .When(HttpMethod.Get, _authMeUrl)
                .Respond("application/json", json);

            //Act
            ClientPrincipal? user = await _userProvider.GetCurrentUserAsync();

               
            
            //Assert
            user.Should().NotBeNull();
            user!.IdentityProvider.Should().Be(principal.IdentityProvider);
            user.UserId.Should().Be(principal.UserId);
            user.UserDetails.Should().Be(principal.UserDetails);
            user.UserRoles.Should().BeEquivalentTo(principal.UserRoles);
        }
        
        
    }
}