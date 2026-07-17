# Adapter-Initialisierung: Handel 212

Der folgende Code zeigt, wie [Trading212MessageAdapter](xref:StockSharp.Trading212.Trading212MessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new Trading212MessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Wert>".ToSecureString(),
	ApiSecret = "<Wert>".ToSecureString(),
	IsDemo = true,
	PollingInterval = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
