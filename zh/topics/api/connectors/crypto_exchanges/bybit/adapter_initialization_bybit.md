# ByBit 适配器初始化

下面的代码演示如何初始化 [ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

另一种更方便的方式是使用 `AddAdapter<T>()` 扩展方法：

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<Your API Key>".To<SecureString>();
	a.Secret = "<Your API Secret>".To<SecureString>();
});
```

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
