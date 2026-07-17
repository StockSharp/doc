# Adapter-Initialisierung: Dukascopy JForex

Der folgende Code zeigt, wie [DukasCopyMessageAdapter](xref:StockSharp.DukasCopy.DukasCopyMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	UserName = "<Wert>",
	BridgeJarPath = "<Wert>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
