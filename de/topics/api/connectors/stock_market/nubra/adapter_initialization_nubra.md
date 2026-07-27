# Adapter initialisieren: Nubra

Der folgende Code initialisiert [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new NubraMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	DeviceId = "<id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_nubra.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_nubra.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
