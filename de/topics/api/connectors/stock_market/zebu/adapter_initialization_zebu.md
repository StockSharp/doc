# Adapter initialisieren: Zebu

Der folgende Code initialisiert [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new ZebuMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	UserId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_zebu.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_zebu.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
