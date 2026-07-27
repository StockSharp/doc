# Inicialización del adaptador: EDINET

El siguiente código inicializa [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EdinetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_edinet.md).

## Véase también

[Configuración del conector](configuration_edinet.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
