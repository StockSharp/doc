# Adapter-Initialisierung: Capital Futures

Der folgende Code zeigt, wie [CapitalFuturesMessageAdapter](xref:StockSharp.CapitalFutures.CapitalFuturesMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new CapitalFuturesMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	SdkPath = "<Wert>",
	Login = "<Wert>",
	Account = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
