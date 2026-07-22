# Inicialización del adaptador ProBit Global

El siguiente código muestra cómo inicializar [ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su identificador de cliente OAuth>".To<SecureString>(),
	Secret = "<Su secreto de cliente OAuth>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Omita `Key` y `Secret` cuando solo necesite datos públicos de mercado.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
