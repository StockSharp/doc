# Kotak Neo 适配器初始化

以下代码演示如何初始化 [KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<消费者密钥>".ToSecureString(),
	MobileNumber = "<手机号码>",
	UserCode = "<用户代码>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<TOTP 密钥>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
