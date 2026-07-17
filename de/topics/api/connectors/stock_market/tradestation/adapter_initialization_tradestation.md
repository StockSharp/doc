# Initialisierung des TradeStation-Adapters

Der folgende Code zeigt, wie [TradeStationMessageAdapter](xref:StockSharp.TradeStation.TradeStationMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new TradeStationMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	IsDemo = true,
	DefaultRoute = "Intelligent",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
