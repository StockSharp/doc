# Adapter initialisieren: Bigul

Der folgende Code initialisiert [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_bigul.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_bigul.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
