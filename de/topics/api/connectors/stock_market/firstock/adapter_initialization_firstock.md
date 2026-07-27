# Adapter initialisieren: Firstock

Der folgende Code initialisiert [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new FirstockMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	Password = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	VendorCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_firstock.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_firstock.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
