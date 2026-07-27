# Adapter initialisieren: DNSE

Der folgende Code initialisiert [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new DnseMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	TradingToken = "<token>".ToSecureString(),
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_dnse.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_dnse.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
