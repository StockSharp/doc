# Adapter-Initialisierung: Fubon Neo

Der folgende Code zeigt, wie [FubonNeoMessageAdapter](xref:StockSharp.FubonNeo.FubonNeoMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new FubonNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	ApiKey = "<Wert>".ToSecureString(),
	CertificatePassword = "<Wert>".ToSecureString(),
	SdkPath = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
