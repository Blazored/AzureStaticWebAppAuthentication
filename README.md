# Blazored.AzureStaticWebAppAuthentication
Some helper libraries for using Blazor with azure static web apps.

* Live Demo: add-live-demo-link

## Getting Started

Install Nuget package ...........

Add the following code to your `Program.cs` file for your blazor wasm application.

```csharp
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddAuthorizationCore();
        builder.Services.AddBlazorAzureStaticWebApp();

        await builder.Build().RunAsync();
    }
}
```

This will add the authorization services to blazor as well as a specific `AuthenticationStateProvider` instance for use with Azure Static Web Apps.

The standard authentication method for Blazor can now be added to the `App.razor` file. See the example below.

```razor
<CascadingAuthenticationState>
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)"/>
        </Found>
        <NotFound>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </NotFound>
    </Router>
</CascadingAuthenticationState>
```

This will now allow the standard `[Authorize]` attribute to be used on pages that required authorization or the `<AuthorizeView/>` provided by the blazor authorization package to optionally show content in a component depending on the authorization state of a user.

## Azure Static Web Apps Authentication Overview

Azure static web apps provides a simple and clean way to allow your uses to authenticate when using a web app. It offers authentication from popular providers such as Azure Active Directory, GitHub & Twitter.

Once authenticated then a user can be authorized within your static web app using a set of helper endpoints. More detail on this is provided on Microsoft's documentation [here](https://docs.microsoft.com/en-us/azure/static-web-apps/authentication-authorization).

## Authenticating with a Provider

The package also includes a class that allows you to easily allow a user to login using one of the default providers. Simply inject an instance of the `IAzureStaticWebAppNavigationService` and use the fluent API to login a user. See below for a sample.

```csharp
void GitHubLogin() =>
        _azureStaticWebAppNavigationService.Login()
            .WithGithub()
            .Navigate();
    
void TwitterLogin() =>
    _azureStaticWebAppNavigationService.Login()
        .WithTwitter()
        .Navigate();

void MicrosoftLogin() =>
    _azureStaticWebAppNavigationService.Login()
        .WithAzureActiveDirectory()
        .Navigate();
```

The above code takes your user to the given authentication provider where they can provide there credentials. Then a user is redirected back to your site. The default behavior is to redirect the user back the route marked with `/` in your Blazor application. The API also provides a way to specify an optional redirect URL to any of the providers where your user will be redirected to once logged in successfully see the example below.

```csharp
void GitHubLogin() =>
        _azureStaticWebAppNavigationService.Login()
            .WithGithub()
            .WithRedirect("http://test.com")
            .Navigate();
```

## The Sample

The sample provides a clear and concise example of how this package can be used to perform the basics operations on a user using this library in Azure Static Web Apps.

<!-- TODO: Add Url once deployed.-->
The example application running on Azure Static Web Apps can be accessed [here]()

### Local Development Setup

The application can also be run locally if used with Azure Static Web Apps cli tool. Details on how to set this up are shown below.

### Sample Overview

The initial page of the demo application gives a bit of an overview of what this package offers. In the top nav their is an option to view all of the login providers. Clicking this link will take you to the `ProvidersPage.razor` where it allows you to login using any of the providers listed clicking any of the buttons will call the relevant method's as shown below.

```csharp
void GitHubLogin() =>
        _azureStaticWebAppNavigationService.Login()
            .WithGithub()
            .Navigate();
    
void TwitterLogin() =>
    _azureStaticWebAppNavigationService.Login()
        .WithTwitter()
        .Navigate();

void MicrosoftLogin() =>
    _azureStaticWebAppNavigationService.Login()
        .WithAzureActiveDirectory()
        .Navigate();
```

Once logged into one of the providers you will notice two more options become available in the top nav allowing you to view your user information using two different methods and also allowing you to log out.

The user profile page for both methods under the hood make use of the special `.auth/me` endpoint provided by Azure Static Web Apps to access the user's information but the way the information is gathered is slightly different.

For more information on the `.auth/me` endpoint see [here](https://docs.microsoft.com/en-us/azure/static-web-apps/user-information?tabs=javascript).

#### Cascading Authentication State
Blazor's authorization package provides an option to include the `AuthenticationStateProvider` as a cascading parameter, making it available to all component's.

See [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-5.0) for more information on cascading parameters and [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-5.0#expose-the-authentication-state-as-a-cascading-parameter-1) for more information on the `<CascadingAuthenticationState>`.

#### Azure Static Web Apps .auth/me









