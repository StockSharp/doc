# ByBit-Adapterinitialisierung

Der folgende Code zeigt, wie der [ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
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
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<Ihr API-Schlüssel>".To<SecureString>();
	a.Secret = "<Ihr API-Geheimnis>".To<SecureString>();
});
```

## Siehe auch

[Fenster der Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
