> [!CAUTION]
> **La bolsa FTX ha cesado sus operaciones. El conector ya no funciona; la documentación se conserva únicamente como referencia.**

# Inicialización del adaptador FTX

El siguiente código muestra cómo inicializar [FtxMessageAdapter](xref:StockSharp.FTX.FtxMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FtxMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su valor>".To<SecureString>(),
	Secret = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
