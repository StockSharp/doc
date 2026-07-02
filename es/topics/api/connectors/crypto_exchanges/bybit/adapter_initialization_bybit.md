# Inicialización del adaptador ByBit

El siguiente código demuestra cómo inicializar el [ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter) y pasarlo al [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Una forma alternativa y más conveniente es utilizar el método de extensión `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<Your API Key>".To<SecureString>();
	a.Secret = "<Your API Secret>".To<SecureString>();
});
```

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
