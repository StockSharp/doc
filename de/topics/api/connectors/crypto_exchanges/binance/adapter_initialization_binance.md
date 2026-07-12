# Binance Adapterinitialisierung

Der folgende Code zeigt, wie [BinanceMessageAdapter](xref:StockSharp.Binance.BinanceMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BinanceMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Eine alternative und bequemere Möglichkeit ist die Verwendung der Erweiterungsmethode `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<BinanceMessageAdapter>(a =>
{
	a.Key = "<Ihr API-Schlüssel>".To<SecureString>();
	a.Secret = "<Ihr API-Geheimnis>".To<SecureString>();
});
```

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
