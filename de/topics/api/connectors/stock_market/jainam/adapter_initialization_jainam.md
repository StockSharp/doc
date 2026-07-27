# Adapter initialisieren: Jainam

Der folgende Code initialisiert [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new JainamMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	AppCode = "<id>",
	ApiSecret = "<secret>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_jainam.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_jainam.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
