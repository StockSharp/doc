# Adapter initialisieren: Rupeezy

Der folgende Code initialisiert [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new RupeezyMessageAdapter(Connector.TransactionIdGenerator)
{
	ApplicationId = "<id>",
	ApiKey = "<key>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_rupeezy.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_rupeezy.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
