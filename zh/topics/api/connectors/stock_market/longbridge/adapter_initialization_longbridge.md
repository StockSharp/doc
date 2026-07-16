# Longbridge OpenAPI 适配器初始化

以下代码演示如何初始化 [LongbridgeMessageAdapter](xref:StockSharp.Longbridge.LongbridgeMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new LongbridgeMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<应用程序密钥>",
	AppSecret = "<应用程序秘密>".ToSecureString(),
	AccessToken = "<访问令牌>".ToSecureString(),
	Portfolio = "Longbridge",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)

