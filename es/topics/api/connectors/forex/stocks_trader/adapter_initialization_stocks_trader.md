# Inicialización del adaptador: StocksTrader

El siguiente código inicializa [StocksTraderMessageAdapter](xref:StockSharp.StocksTrader.StocksTraderMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StocksTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por el token emitido para la cuenta demo o real seleccionada.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
