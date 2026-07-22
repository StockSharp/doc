# Zero Hash 适配器初始化

下面的代码演示如何初始化 [ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<您的值>",
	Secret = "<您的值>".To<SecureString>(),
	Passphrase = "<您的值>".To<SecureString>(),
	Account = "<您的值>",
	User = "<您的值>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
