# 适配器初始化：Coinalyze

以下代码初始化 [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinalyzeMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<您的访问令牌>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_coinalyze.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_coinalyze.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
