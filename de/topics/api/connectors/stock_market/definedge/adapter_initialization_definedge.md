# Adapter initialisieren: Definedge

Der folgende Code initialisiert [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_definedge.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_definedge.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
