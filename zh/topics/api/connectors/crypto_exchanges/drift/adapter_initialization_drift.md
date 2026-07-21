# Drift 适配器初始化

下面的代码演示如何初始化 [DriftMessageAdapter](xref:StockSharp.Drift.DriftMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DriftMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<您的值>",
	PrivateKey = "<您的值>".To<SecureString>(),
	AccountAddress = "<您的值>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
