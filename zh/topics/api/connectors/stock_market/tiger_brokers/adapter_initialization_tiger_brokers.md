# Tiger Brokers 适配器初始化

以下代码演示如何初始化 [TigerBrokersMessageAdapter](xref:StockSharp.TigerBrokers.TigerBrokersMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TigerBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	TigerId = "<Tiger 标识符>",
	Account = "<账户>",
	License = TigerLicenses.Singapore,
	PrivateKey = "<私钥>".ToSecureString(),
	Token = "<令牌>".ToSecureString(),
	AutoGrabPermission = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)

