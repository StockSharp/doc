# 适配器初始化：AltCoinTrader

以下代码初始化 [AltCoinTraderMessageAdapter](xref:StockSharp.AltCoinTrader.AltCoinTraderMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new AltCoinTraderMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_altcointrader.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_altcointrader.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
