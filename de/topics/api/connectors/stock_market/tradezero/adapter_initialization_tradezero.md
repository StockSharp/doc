# Initialisierung des TradeZero-Adapters

Der folgende Code zeigt, wie [TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<optionale Orderroute>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

`DefaultRoute` kann weggelassen werden, damit der Konnektor automatisch eine kompatible Route auswählt.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
