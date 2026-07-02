# FIX 适配器初始化

下面的代码演示了如何初始化 [FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

另一种更方便的方式是使用 `AddAdapter<T>()` 扩展方法：

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<FixMessageAdapter>(a =>
{
	a.Login = "<Your Login>";
	a.Password = "<Your Password>".To<SecureString>();
	a.Address = "<Address>".To<EndPoint>();
});
```

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
