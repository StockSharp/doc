# OAuth 授权

要在程序中的连接器中使用 OAuth 授权，您需要注册几个服务：

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

- [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) 和 [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) 实现了 [ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider) 接口。
- [ApiServiceProvider](xref:StockSharp.Web.Api.Client.ApiServiceProvider) 实现了 [IApiServiceProvider](xref:StockSharp.Web.Api.Client.IApiServiceProvider) 接口。
- `OAuthProvider` 实现了 [IOAuthProvider](xref:Ecng.Net.IOAuthProvider) 接口。

[ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider)有两种实现选项：

1. [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) - 从本地文件加载StockSharp账户数据。需要事先授权。例如，通过安装程序。

2. [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) - 直接从代码传递令牌。不需要在机器上使用密钥文件。令牌是从 [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/) 获取的：

   ![配置文件](../../../images/profile.png)

有两种实现[IOAuthProvider](xref:Ecng.Net.IOAuthProvider)的选项:

1. [WebApiOAuthProvider](xref:StockSharp.Studio.WebApi.WebApiOAuthProvider) - 适用于不需要显示授权窗口的控制台应用程序。

2. [OAuthProvider](xref:StockSharp.Studio.Controls.OAuthProvider) - 用于需要显示授权窗口的 WPF 应用程序：

   ![授权开始](../../../images/oauth_start.png)