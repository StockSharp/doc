# Adapter initialisieren: Ventura

Der folgende Code initialisiert [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_ventura.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_ventura.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
