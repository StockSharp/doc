# Adapter-Initialisierung: Motilal Oswal

Der folgende Code zeigt, wie [MotilalOswalMessageAdapter](xref:StockSharp.MotilalOswal.MotilalOswalMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new MotilalOswalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Wert>".ToSecureString(),
	Secret = "<Wert>".ToSecureString(),
	Token = "<Wert>".ToSecureString(),
	AccessToken = "<Wert>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
