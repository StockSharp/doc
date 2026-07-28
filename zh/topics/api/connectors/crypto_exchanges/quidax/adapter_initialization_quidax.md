# 适配器初始化：Quidax

以下代码初始化 [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<您的访问令牌>".To<SecureString>(),
	UserId = "<您的用户标识>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_quidax.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_quidax.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
