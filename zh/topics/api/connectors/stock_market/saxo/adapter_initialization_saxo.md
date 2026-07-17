# Saxo OpenAPI 适配器初始化

以下代码演示如何初始化 [SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<访问令牌>".ToSecureString(),
	RefreshToken = "<刷新令牌>".ToSecureString(),
	ClientId = "<客户端标识符>",
	ClientSecret = "<客户端密钥>".ToSecureString(),
	RedirectUri = "<重定向 URI>",
	AccountKey = "<账户密钥>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
