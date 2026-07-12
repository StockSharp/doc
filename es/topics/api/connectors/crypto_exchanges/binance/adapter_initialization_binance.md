# Inicialización del adaptador Binance

El código a continuación muestra cómo inicializar [BinanceMessageAdapter](xref:StockSharp.Binance.BinanceMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BinanceMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Una forma alternativa y más conveniente es usar el método de extensión `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<BinanceMessageAdapter>(a =>
{
	a.Key = "<Su clave API>".To<SecureString>();
	a.Secret = "<Su secreto API>".To<SecureString>();
});
```

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
