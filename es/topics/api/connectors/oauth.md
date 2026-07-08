# Autorización OAuth

Para usar autorización OAuth en conectores dentro de su programa, debe registrar varios servicios:

```csharp
// Connecting the password storage for access to StockSharp WebAPI.
ConfigManager.RegisterService<ICredentialsProvider>(new DefaultCredentialsProvider());
//ConfigManager.RegisterService<ICredentialsProvider>(new TokenCredentialsProvider("%token%"));

// Connecting the service provider for access to StockSharp WebAPI
ConfigManager.RegisterService<IApiServiceProvider>(new ApiServiceProvider());
							
// Servicio de autorización OAuth que usarán los conectores
ConfigManager.RegisterService<IOAuthProvider>(new OAuthProvider());
//ConfigManager.RegisterService<IOAuthProvider>(new WebApiOAuthProvider());
```

- [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) y [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) implementan la interfaz [ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider).
- [ApiServiceProvider](xref:StockSharp.Web.Api.Client.ApiServiceProvider) implementa la interfaz [IApiServiceProvider](xref:StockSharp.Web.Api.Client.IApiServiceProvider).
- `OAuthProvider` implementa la interfaz [IOAuthProvider](xref:Ecng.Net.IOAuthProvider).

Hay dos opciones de implementación para [ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider):

1. [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) - carga los datos de la cuenta StockSharp desde un archivo local. Se requiere autorización previa. Por ejemplo, mediante Installer.

2. [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) - pasa el token directamente desde el código. No se requiere un archivo secreto en la máquina. El token se obtiene en [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

   ![Profile](../../../images/profile.png)

Hay dos opciones de implementación para [IOAuthProvider](xref:Ecng.Net.IOAuthProvider):

1. [WebApiOAuthProvider](xref:StockSharp.Studio.WebApi.WebApiOAuthProvider) - para aplicaciones de consola donde no se requiere mostrar una ventana de autorización.

2. [OAuthProvider](xref:StockSharp.Studio.Controls.OAuthProvider) - para aplicaciones WPF donde debe mostrarse una ventana de autorización:

   ![OAuth Start](../../../images/oauth_start.png)
