# Adapter-Initialisierung: kabu Station

Der folgende Code zeigt, wie [KabuStationMessageAdapter](xref:StockSharp.KabuStation.KabuStationMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new KabuStationMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiPassword = "<Wert>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
