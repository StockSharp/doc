# 适配器初始化: Mirae Asset Sharekhan

以下代码演示如何初始化 [MiraeSharekhanMessageAdapter](xref:StockSharp.MiraeSharekhan.MiraeSharekhanMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new MiraeSharekhanMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<值>".ToSecureString(),
	ApiKey = "<值>",
	VendorKey = "<值>",
	CustomerId = "<值>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
