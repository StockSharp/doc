# 适配器初始化：InvertirOnline

以下代码初始化 [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new InvertirOnlineMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_invertironline.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_invertironline.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
