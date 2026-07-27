# Inicialización del adaptador: Korean FSC

El siguiente código inicializa [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KoreanFscMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_korean_fsc.md).

## Véase también

[Configuración del conector](configuration_korean_fsc.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
