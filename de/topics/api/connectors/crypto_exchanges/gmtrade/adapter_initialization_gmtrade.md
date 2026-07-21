# Initialisierung des GMTrade-Adapters

Der folgende Code zeigt, wie man den [GMTradeMessageAdapter](xref:StockSharp.GMTrade.GMTradeMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GMTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihr Wert>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
