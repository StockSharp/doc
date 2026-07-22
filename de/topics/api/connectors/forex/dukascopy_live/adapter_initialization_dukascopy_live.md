# Adapter-Initialisierung: DukasCopy Live

Der folgende Code zeigt, wie [DukasCopyLiveMessageAdapter](xref:StockSharp.DukasCopyLive.DukasCopyLiveMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new DukasCopyLiveMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	Login = "<Wert>",
	BridgeJarPath = "<Wert>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
