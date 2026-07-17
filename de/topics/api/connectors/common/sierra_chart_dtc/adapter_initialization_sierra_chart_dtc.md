# Adapter-Initialisierung: Sierra Chart DTC

Der folgende Code zeigt, wie [SierraChartDtcMessageAdapter](xref:StockSharp.SierraChartDtc.SierraChartDtcMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new SierraChartDtcMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	Login = "<Wert>",
	TradeAccount = "<Wert>",
	TargetHost = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
