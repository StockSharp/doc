# Coinbase-Adapterinitialisierung

Der folgende Code zeigt, wie der [CoinbaseMessageAdapter](xref:StockSharp.Coinbase.CoinbaseMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
var messageAdapter = new CoinbaseMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

Eine alternative und bequemere Methode ist die Verwendung der Erweiterungsmethode `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<CoinbaseMessageAdapter>(a =>
{
	a.Key = "<Ihr API-Schlüssel>".To<SecureString>();
	a.Secret = "<Ihr API-Geheimnis>".To<SecureString>();
});
```

## Siehe auch

[Fenster der Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
