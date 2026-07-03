# Rithmic 适配器初始化

下面的代码演示如何初始化 [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	CertFile = "<Path to certificate file>",
	Server = RithmicServers.Real,
	//Server = RithmicServers.Test,
	//Server = RithmicServers.Simulator,  
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

另一种更方便的方式是使用 `AddAdapter<T>()` 扩展方法：

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<RithmicMessageAdapter>(a =>
{
	a.UserName = "<Your Login>";
	a.Password = "<Your Password>".To<SecureString>();
	a.CertFile = "<Path to certificate file>";
	a.Server = RithmicServers.Real;
});
```

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
