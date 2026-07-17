# Adapter-Initialisierung: OpenMarkets

Der folgende Code zeigt, wie [OpenMarketsMessageAdapter](xref:StockSharp.OpenMarkets.OpenMarketsMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new OpenMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientSecret = "<Wert>".ToSecureString(),
	ClientId = "<Wert>",
	AccountCode = "<Wert>",
	DataSource = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
