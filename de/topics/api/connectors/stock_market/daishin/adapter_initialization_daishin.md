# Adapter-Initialisierung: Daishin CYBOS Plus

Der folgende Code zeigt, wie [DaishinMessageAdapter](xref:StockSharp.Daishin.DaishinMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new DaishinMessageAdapter(Connector.TransactionIdGenerator)
{
	Account = "<Wert>",
	IsTradingEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
