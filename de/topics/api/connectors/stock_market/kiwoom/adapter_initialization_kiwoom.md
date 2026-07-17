# Adapter-Initialisierung: Kiwoom

Der folgende Code zeigt, wie [KiwoomMessageAdapter](xref:StockSharp.Kiwoom.KiwoomMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new KiwoomMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Wert>".ToSecureString(),
	AppSecret = "<Wert>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
