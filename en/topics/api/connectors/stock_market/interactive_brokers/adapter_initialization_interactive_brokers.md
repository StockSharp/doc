# Interactive Brokers Adapter Initialization

The code below demonstrates how to initialize [InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

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

An alternative and more convenient way is to use the `AddAdapter<T>()` extension method:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<InteractiveBrokersMessageAdapter>(a =>
{
	a.Address = "<Your Address>".To<EndPoint>();
});
```

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
