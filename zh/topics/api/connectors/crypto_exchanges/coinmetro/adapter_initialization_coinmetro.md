# 适配器初始化：Coinmetro

以下代码初始化 [CoinmetroMessageAdapter](xref:StockSharp.Coinmetro.CoinmetroMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinmetroMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<您的访问令牌>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_coinmetro.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_coinmetro.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
