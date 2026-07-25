# 适配器初始化：MetaApi

以下代码初始化 [MetaApiMessageAdapter](xref:StockSharp.MetaApi.MetaApiMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new MetaApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为在 MetaApi 中部署的账户的令牌和标识符。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
