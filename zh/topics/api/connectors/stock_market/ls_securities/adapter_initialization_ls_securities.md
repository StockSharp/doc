# 适配器初始化: LS Securities

以下代码演示如何初始化 [LsSecuritiesMessageAdapter](xref:StockSharp.LsSecurities.LsSecuritiesMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new LsSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<值>".ToSecureString(),
	AppSecret = "<值>".ToSecureString(),
	Account = "<值>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
