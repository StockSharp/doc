# Inicialización del adaptador Rithmic

El siguiente código muestra cómo inicializar [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Su usuario>",
	Password = "<Su contraseña>".To<SecureString>(),
	CertFile = "<Ruta al archivo de certificado>",
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
	a.UserName = "<Su usuario>";
	a.Password = "<Su contraseña>".To<SecureString>();
	a.CertFile = "<Ruta al archivo de certificado>";
	a.Server = RithmicServers.Real;
});
```

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
