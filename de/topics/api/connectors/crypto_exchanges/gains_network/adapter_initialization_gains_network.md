# Initialisierung des Gains Network-Adapters

Der folgende Code zeigt, wie man den [GainsNetworkMessageAdapter](xref:StockSharp.GainsNetwork.GainsNetworkMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GainsNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
