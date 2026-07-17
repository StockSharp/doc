# Adapter-Initialisierung: lemon.markets

Der folgende Code zeigt, wie [LemonMarketsMessageAdapter](xref:StockSharp.LemonMarkets.LemonMarketsMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new LemonMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Wert>".ToSecureString(),
	AccountId = "<Wert>",
	SecuritiesAccountId = "<Wert>",
	DataPrivacyPrincipal = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
