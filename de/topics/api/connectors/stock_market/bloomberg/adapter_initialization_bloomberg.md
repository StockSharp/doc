# Adapter-Initialisierung: Bloomberg BLPAPI and EMSX

Der folgende Code zeigt, wie [BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<Wert>",
	EmsxService = "<Wert>",
	Broker = "<Wert>",
	ServerAddress = "<Wert>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
