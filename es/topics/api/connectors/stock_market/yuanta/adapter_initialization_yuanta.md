# Inicialización del adaptador: Yuanta SPARK

El código siguiente muestra cómo inicializar [YuantaMessageAdapter](xref:StockSharp.Yuanta.YuantaMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new YuantaMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	CertificatePassword = "<valor>".ToSecureString(),
	SdkPath = "<valor>",
	Account = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
