# Inicialización del adaptador: AltCoinTrader

El siguiente código inicializa [AltCoinTraderMessageAdapter](xref:StockSharp.AltCoinTrader.AltCoinTraderMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new AltCoinTraderMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_altcointrader.md).

## Véase también

[Configuración del conector](configuration_altcointrader.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
