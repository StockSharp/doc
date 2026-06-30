# FIX Adapter Initialization

The code below demonstrates how to initialize [FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

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

An alternative and more convenient way is to use the `AddAdapter<T>()` extension method:

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

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
