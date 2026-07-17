# Adapter-Initialisierung: Handelstechnologien

Der folgende Code zeigt, wie [TradingTechnologiesMessageAdapter](xref:StockSharp.TradingTechnologies.TradingTechnologiesMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new TradingTechnologiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppSecretKey = "<Wert>".ToSecureString(),
	SdkPath = "<Wert>",
	IsBinaryProtocol = true,
	IsOptionsEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
