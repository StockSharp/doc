# Adapter-Initialisierung: Zerodha Kite Connect

Der folgende Code zeigt, wie [ZerodhaMessageAdapter](xref:StockSharp.Zerodha.ZerodhaMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new ZerodhaMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiSecret = "<Wert>".ToSecureString(),
	Token = "<Wert>".ToSecureString(),
	RequestToken = "<Wert>".ToSecureString(),
	ApiKey = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
