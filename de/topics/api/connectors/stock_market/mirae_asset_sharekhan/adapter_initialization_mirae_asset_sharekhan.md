# Adapter-Initialisierung: Mirae Asset Sharekhan

Der folgende Code zeigt, wie [MiraeSharekhanMessageAdapter](xref:StockSharp.MiraeSharekhan.MiraeSharekhanMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new MiraeSharekhanMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Wert>".ToSecureString(),
	ApiKey = "<Wert>",
	VendorKey = "<Wert>",
	CustomerId = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
