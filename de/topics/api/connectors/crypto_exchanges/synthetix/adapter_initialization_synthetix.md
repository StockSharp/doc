# Initialisierung des Synthetix-Adapters

Der folgende Code zeigt, wie man den [SynthetixMessageAdapter](xref:StockSharp.Synthetix.SynthetixMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SynthetixMessageAdapter(Connector.TransactionIdGenerator)
{
	SubAccountId = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
