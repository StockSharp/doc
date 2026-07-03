# Inicialización del adaptador CQG

El siguiente código muestra cómo inicializar [CqgComMessageAdapter](xref:StockSharp.Cqg.Com.CqgComMessageAdapter) y [CqgContinuumMessageAdapter](xref:StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter), y enviarlos a [Connector](xref:StockSharp.Algo.Connector).

1. **CQG COM**, conexión mediante **CQG Integrated Client** local:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

2. **CQG Continuum**, conexión directa al servidor:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Address>".To<IPAddress>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
