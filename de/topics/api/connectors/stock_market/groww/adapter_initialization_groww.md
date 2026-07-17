# Adapter-Initialisierung: Groww

Der folgende Code zeigt, wie [GrowwMessageAdapter](xref:StockSharp.Groww.GrowwMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new GrowwMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Wert>".ToSecureString(),
	ApiKey = "<Wert>".ToSecureString(),
	ApiSecret = "<Wert>".ToSecureString(),
	TotpSecret = "<Wert>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
