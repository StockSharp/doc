# Initialisierung des Polymarket-Adapters

Der folgende Code zeigt, wie man den [PolymarketMessageAdapter](xref:StockSharp.Polymarket.PolymarketMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PolymarketMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr Wert>",
	ApiSecret = "<Ihr Wert>".To<SecureString>(),
	Passphrase = "<Ihr Wert>".To<SecureString>(),
	SignerAddress = "<Ihr Wert>",
	FunderAddress = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
