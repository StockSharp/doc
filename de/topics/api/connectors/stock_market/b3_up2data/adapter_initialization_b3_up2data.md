# Adapter initialisieren: B3 UP2DATA

Der folgende Code initialisiert [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new B3Up2DataMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_b3_up2data.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_b3_up2data.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
