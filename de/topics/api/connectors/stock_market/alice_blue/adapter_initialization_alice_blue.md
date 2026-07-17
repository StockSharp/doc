# Adapter-Initialisierung: Alice Blue

Der folgende Code zeigt, wie [AliceBlueMessageAdapter](xref:StockSharp.AliceBlue.AliceBlueMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new AliceBlueMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Wert>".ToSecureString(),
	UserId = "<Wert>",
	ClientId = "<Wert>",
	DeviceId = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
