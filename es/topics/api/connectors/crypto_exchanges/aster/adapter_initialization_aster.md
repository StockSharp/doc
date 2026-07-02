# Inicialización del adaptador Aster

El código a continuación muestra cómo inicializar [AsterMessageAdapter](xref:StockSharp.Aster.AsterMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AsterMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	Section = AsterSections.Derivatives,
	DerivativesProtocolMode = AsterDerivativesProtocolModes.Legacy,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
