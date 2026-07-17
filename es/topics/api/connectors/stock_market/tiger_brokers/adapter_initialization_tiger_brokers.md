# Inicialización del adaptador Tiger Brokers

El código siguiente muestra cómo inicializar [TigerBrokersMessageAdapter](xref:StockSharp.TigerBrokers.TigerBrokersMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TigerBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	TigerId = "<Identificador de Tiger>",
	Account = "<Cuenta>",
	License = TigerLicenses.Singapore,
	PrivateKey = "<Clave privada>".ToSecureString(),
	Token = "<Token>".ToSecureString(),
	AutoGrabPermission = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
