# Initialisierung des Orderly Network-Adapters

Der folgende Code zeigt, wie man den [OrderlyNetworkMessageAdapter](xref:StockSharp.OrderlyNetwork.OrderlyNetworkMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OrderlyNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	AccountId = "<Ihr Wert>",
	Secret = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
