# Interactive Brokers 适配器初始化

下面的代码演示如何初始化 [InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Your Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

另一种更方便的方式是使用 `AddAdapter<T>()` 扩展方法：

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<InteractiveBrokersMessageAdapter>(a =>
{
	a.Address = "<Your Address>".To<EndPoint>();
});
```

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
