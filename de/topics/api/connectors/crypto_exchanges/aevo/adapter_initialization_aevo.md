# Initialisierung des Aevo-Adapters

Der folgende Code zeigt, wie man den [AevoMessageAdapter](xref:StockSharp.Aevo.AevoMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AevoMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr Wert>",
	ApiSecret = "<Ihr Wert>".To<SecureString>(),
	WalletAddress = "<Ihr Wert>",
	SigningKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
