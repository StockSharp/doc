# Inicialización del adaptador Coinbase

El código siguiente muestra cómo inicializar [CoinbaseMessageAdapter](xref:StockSharp.Coinbase.CoinbaseMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
var messageAdapter = new CoinbaseMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

Una forma alternativa y más conveniente es usar el método de extensión `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<CoinbaseMessageAdapter>(a =>
{
	a.Key = "<Your API Key>".To<SecureString>();
	a.Secret = "<Your API Secret>".To<SecureString>();
});
```

## Véase también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
