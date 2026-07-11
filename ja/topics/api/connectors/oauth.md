# OAuth 認可

プログラム内のコネクターで OAuth 認可を使用するには、いくつかのサービスを登録する必要があります。

```csharp
// StockSharp WebAPI へアクセスするためのパスワードストレージを接続する。
ConfigManager.RegisterService<ICredentialsProvider>(new DefaultCredentialsProvider());
//ConfigManager.RegisterService<ICredentialsProvider>(new TokenCredentialsProvider("%token%"));

// StockSharp WebAPI へアクセスするためのサービスプロバイダーを接続する
ConfigManager.RegisterService<IApiServiceProvider>(new ApiServiceProvider());
							
// コネクターによって使用される OAuth 認可サービス
ConfigManager.RegisterService<IOAuthProvider>(new OAuthProvider());
//ConfigManager.RegisterService<IOAuthProvider>(new WebApiOAuthProvider());
```

- [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) と [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) は、[ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider) インターフェイスを実装します。
- [ApiServiceProvider](xref:StockSharp.Web.Api.Client.ApiServiceProvider) は、[IApiServiceProvider](xref:StockSharp.Web.Api.Client.IApiServiceProvider) インターフェイスを実装します。
- `OAuthProvider` は、[IOAuthProvider](xref:Ecng.Net.IOAuthProvider) インターフェイスを実装します。

[ICredentialsProvider](xref:StockSharp.Configuration.ICredentialsProvider) には 2 つの実装オプションがあります。

1. [DefaultCredentialsProvider](xref:StockSharp.Configuration.DefaultCredentialsProvider) - ローカルファイルから StockSharp アカウントデータを読み込みます。事前の認可が必要です。たとえば、Installer を通じて行います。

2. [TokenCredentialsProvider](xref:StockSharp.Configuration.TokenCredentialsProvider) - コードからトークンを直接渡します。マシン上にシークレットファイルは不要です。トークンは [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/) から取得します。

   ![プロファイル](../../../images/profile.png)

[IOAuthProvider](xref:Ecng.Net.IOAuthProvider) には 2 つの実装オプションがあります。

1. [WebApiOAuthProvider](xref:StockSharp.Studio.WebApi.WebApiOAuthProvider) - 認可ウィンドウの表示が不要なコンソールアプリケーション向けです。

2. [OAuthProvider](xref:StockSharp.Studio.Controls.OAuthProvider) - 認可ウィンドウを表示する必要がある WPF アプリケーション向けです。

   ![OAuth 開始](../../../images/oauth_start.png)
