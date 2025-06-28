# OAuth Authorization

To use OAuth authorization in connectors in your program, you need to register several services:

```csharp
// Connecting the password storage for access to StockSharp WebAPI.
ConfigManager.RegisterService<ICredentialsProvider>(new DefaultCredentialsProvider());
//ConfigManager.RegisterService<ICredentialsProvider>(new TokenCredentialsProvider("%token%"));

// Connecting the service provider for access to StockSharp WebAPI
ConfigManager.RegisterService<IApiServiceProvider>(new ApiServiceProvider());
							
// OAuth authorization service that will be used by connectors
ConfigManager.RegisterService<IOAuthProvider>(new OAuthProvider());
//ConfigManager.RegisterService<IOAuthProvider>(new WebApiOAuthProvider());
```

- [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) and [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) implement the [ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider) interface.
- [ApiServiceProvider](xref:StockSharp.Web.Api.Client.ApiServiceProvider) implements the [IApiServiceProvider](xref:StockSharp.Web.Api.Client.IApiServiceProvider) interface.
- `OAuthProvider` implements the [IOAuthProvider](xref:Ecng.Net.IOAuthProvider) interface.

There are two implementation options for [ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider):

1. [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) - loads StockSharp account data from a local file. Prior authorization is required. For example, through the Installer.

2. [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) - passes the token directly from the code. No secret file is required on the machine. The token is obtained from [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

   ![Profile](../../../images/profile.png)

There are two implementation options for [IOAuthProvider](xref:Ecng.Net.IOAuthProvider):

1. [WebApiOAuthProvider](xref:StockSharp.Studio.WebApi.WebApiOAuthProvider) - for console applications where no authorization window display is required.

2. [OAuthProvider](xref:StockSharp.Studio.Controls.OAuthProvider) - for WPF applications where an authorization window needs to be displayed:

   ![OAuth Start](../../../images/oauth_start.png)