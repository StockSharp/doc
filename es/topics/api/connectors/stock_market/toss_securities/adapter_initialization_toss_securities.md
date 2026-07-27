# Inicialización del adaptador: Toss Securities

El siguiente código inicializa [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TossSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_toss_securities.md).

## Véase también

[Configuración del conector](configuration_toss_securities.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
