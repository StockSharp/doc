# Inicialización del adaptador Blackwood (Fusion)

El siguiente código muestra cómo inicializar [BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var address = "<Address>".To<IPAddress>();
var messageAdapter = new BlackwoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	ExecutionAddress = new IPEndPoint(address, BlackwoodAddresses.ExecutionPort),
	MarketDataAddress = new IPEndPoint(address, BlackwoodAddresses.MarketDataPort),
	HistoricalDataAddress = new IPEndPoint(address, BlackwoodAddresses.HistoricalDataPort)
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
