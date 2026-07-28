# 适配器初始化：Pendle

以下代码初始化 [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new PendleMessageAdapter(connector.TransactionIdGenerator)
{
	Chain = PendleChains.Ethereum,
	WalletAddress = "<您的钱包地址>",
	PrivateKey = "<您的私钥>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写钱包凭据以及[连接器配置](configuration_pendle.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_pendle.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
