# 适配器初始化：Settrade

以下代码初始化 [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API 机密>".To<SecureString>(),
	AppCode = "<您的应用程序代码>",
	BrokerId = "<您的经纪商标识符>",
	Account = "<您的账户号码>",
	Pin = "<您的交易 PIN>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据、账户类型以及[连接器配置](configuration_settrade.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_settrade.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
