# Adapter-Initialisierung: QMT

Der folgende Code zeigt, wie [QmtMessageAdapter](xref:StockSharp.Qmt.QmtMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new QmtMessageAdapter(Connector.TransactionIdGenerator)
{
	GatewayToken = "<Wert>".ToSecureString(),
	GatewayHost = "<Wert>",
	GatewayPort = 10,
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
