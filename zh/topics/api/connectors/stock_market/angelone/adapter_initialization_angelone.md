# Angel One 适配器初始化

以下代码演示如何初始化 [AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<用户名>",
	Password = "<密码>".ToSecureString(),
	ApiKey = "<API 密钥>".ToSecureString(),
	TotpSecret = "<TOTP 密钥>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<客户端公共 IP 地址>",
	MacAddress = "<MAC 地址>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
