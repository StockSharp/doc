# Inicialización del adaptador Robinhood

El código siguiente muestra cómo inicializar [RobinhoodMessageAdapter](xref:StockSharp.Robinhood.RobinhoodMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RobinhoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	Address = new Uri("https://agent.robinhood.com/mcp/trading"),
	PollingInterval = TimeSpan.FromSeconds(2),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
