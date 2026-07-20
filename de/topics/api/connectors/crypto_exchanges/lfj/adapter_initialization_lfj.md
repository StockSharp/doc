# Initialisierung des LFJ-Adapters

Der folgende Code zeigt, wie man den [LfjMessageAdapter](xref:StockSharp.Lfj.LfjMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new LfjMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
