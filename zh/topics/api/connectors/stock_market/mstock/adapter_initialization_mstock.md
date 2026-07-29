# 适配器初始化：m.Stock

以下代码初始化 [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	ClientCode = "<您的客户代码>",
	Password = "<您的密码>".To<SecureString>(),
	Otp = "<当前一次性密码>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写访问参数以及[连接器配置](configuration_mstock.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_mstock.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
