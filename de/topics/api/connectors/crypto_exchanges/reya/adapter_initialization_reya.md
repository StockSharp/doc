# Initialisierung des Reya-Adapters

Der folgende Code zeigt, wie man den [ReyaMessageAdapter](xref:StockSharp.Reya.ReyaMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ReyaMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihr Wert>",
	AccountId = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
