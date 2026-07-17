# Adapter-Initialisierung: SnapTrade

Der folgende Code zeigt, wie [SnapTradeMessageAdapter](xref:StockSharp.SnapTrade.SnapTradeMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new SnapTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<Wert>".ToSecureString(),
	UserSecret = "<Wert>".ToSecureString(),
	ClientId = "<Wert>",
	UserId = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
