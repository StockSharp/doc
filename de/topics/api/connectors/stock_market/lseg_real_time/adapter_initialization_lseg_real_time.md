# Adapter-Initialisierung: LSEG Real-Time

Der folgende Code zeigt, wie [LsegRealTimeMessageAdapter](xref:StockSharp.LsegRealTime.LsegRealTimeMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new LsegRealTimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	Secret = "<Wert>".ToSecureString(),
	Address = "<Wert>",
	StandbyAddress = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
