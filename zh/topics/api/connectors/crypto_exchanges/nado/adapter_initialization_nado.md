# Nado 适配器初始化

下面的代码演示如何初始化 [NadoMessageAdapter](xref:StockSharp.Nado.NadoMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NadoMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<您的值>",
	PrivateKey = "<您的值>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
