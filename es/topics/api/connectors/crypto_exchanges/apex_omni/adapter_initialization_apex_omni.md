# Inicialización del adaptador ApeX Omni

El siguiente código muestra cómo inicializar [ApexOmniMessageAdapter](xref:StockSharp.ApexOmni.ApexOmniMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ApexOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su valor>".To<SecureString>(),
	Secret = "<Su valor>".To<SecureString>(),
	Passphrase = "<Su valor>".To<SecureString>(),
	Seeds = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
