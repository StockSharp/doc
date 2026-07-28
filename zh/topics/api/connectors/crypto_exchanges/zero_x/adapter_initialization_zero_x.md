# 适配器初始化：0x

以下代码初始化 [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<您的 API 密钥>".To<SecureString>(),
	WalletAddress = "<您的钱包地址>",
	PrivateKey = "<您的私钥>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_zero_x.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_zero_x.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
