# Initialisierung des Jupiter-Adapters

Der folgende Code zeigt, wie man den [JupiterMessageAdapter](xref:StockSharp.Jupiter.JupiterMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JupiterMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr Wert>".To<SecureString>(),
	WalletAddress = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
