# Adapter-Initialisierung: Korea Investment & Securities

Der folgende Code zeigt, wie [KoreaInvestmentMessageAdapter](xref:StockSharp.KoreaInvestment.KoreaInvestmentMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new KoreaInvestmentMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Wert>".ToSecureString(),
	AppSecret = "<Wert>".ToSecureString(),
	AccountNumber = "<Wert>",
	ProductCode = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
