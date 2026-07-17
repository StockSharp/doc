# Adapter-Initialisierung: Shoonya

Der folgende Code zeigt, wie [ShoonyaMessageAdapter](xref:StockSharp.Shoonya.ShoonyaMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new ShoonyaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Wert>".ToSecureString(),
	UserId = "<Wert>",
	AccountId = "<Wert>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
