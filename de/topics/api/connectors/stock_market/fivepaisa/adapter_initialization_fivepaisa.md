# Adapter-Initialisierung: 5paisa Xstream

Der folgende Code zeigt, wie [FivePaisaMessageAdapter](xref:StockSharp.FivePaisa.FivePaisaMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new FivePaisaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Wert>".ToSecureString(),
	AppKey = "<Wert>",
	ClientCode = "<Wert>",
	AlgoId = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
