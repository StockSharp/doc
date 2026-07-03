# Inicialización del adaptador Rithmic

El siguiente código muestra cómo inicializar [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

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

Una forma alternativa y más cómoda es usar el método de extensión `AddAdapter<T>()`:

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

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
