# Initialisierung des Zero Hash-Adapters

Der folgende Code zeigt, wie man den [ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr Wert>",
	Secret = "<Ihr Wert>".To<SecureString>(),
	Passphrase = "<Ihr Wert>".To<SecureString>(),
	Account = "<Ihr Wert>",
	User = "<Ihr Wert>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
