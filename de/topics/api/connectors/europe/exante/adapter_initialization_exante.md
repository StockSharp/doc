# Adapter initialisieren: EXANTE

Der folgende Code initialisiert [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new ExanteMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
	SummaryCurrency = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_exante.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_exante.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
