# Rithmic Adapter Initialization

The code below demonstrates how to initialize [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

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

An alternative and more convenient way is to use the `AddAdapter<T>()` extension method:

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

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
