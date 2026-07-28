# 适配器初始化：Chainflip

以下代码初始化 [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<您的 EVM 钱包地址>",
	PrivateKey = "<您的 EVM 私钥>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写钱包凭据、目标地址以及[连接器配置](configuration_chainflip.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_chainflip.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
