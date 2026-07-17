# Adapter-Initialisierung: Yuanta SPARK

Der folgende Code zeigt, wie [YuantaMessageAdapter](xref:StockSharp.Yuanta.YuantaMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
var messageAdapter = new YuantaMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<Wert>".ToSecureString(),
	CertificatePassword = "<Wert>".ToSecureString(),
	SdkPath = "<Wert>",
	Account = "<Wert>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen oder konfigurierten Parameter.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
