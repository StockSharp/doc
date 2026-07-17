# Adapter-Initialisierung: Databento

Der folgende Code zeigt, wie [DatabentoMessageAdapter](xref:StockSharp.Databento.DatabentoMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new DatabentoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Wert>".ToSecureString(),
	Dataset = "<Wert>",
	LiveAddress = "<Wert>",
	HistoricalAddress = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
