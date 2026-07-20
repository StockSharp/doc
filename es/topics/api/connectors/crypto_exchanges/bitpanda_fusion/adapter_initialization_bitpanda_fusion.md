# Inicialización del adaptador Bitpanda Fusion

El siguiente código muestra cómo inicializar [BitpandaFusionMessageAdapter](xref:StockSharp.BitpandaFusion.BitpandaFusionMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitpandaFusionMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
