# CQG Web API 适配器初始化

以下代码演示如何初始化 [CqgMessageAdapter](xref:StockSharp.CQG.CqgMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new CqgMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<用户名>",
	Password = "<密码>".ToSecureString(),
	PrivateLabel = "WebAPITest",
	ClientId = "WebAPITest",
	Endpoint = "wss://demoapi.cqg.com:443",
	Portfolio = "<投资组合>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
