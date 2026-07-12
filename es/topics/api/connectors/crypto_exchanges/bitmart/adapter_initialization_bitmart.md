# Inicialización del adaptador Bitmart

El código siguiente muestra cómo inicializar el [BitmartMessageAdapter](xref:StockSharp.Bitmart.BitmartMessageAdapter) y enviarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitmartMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
