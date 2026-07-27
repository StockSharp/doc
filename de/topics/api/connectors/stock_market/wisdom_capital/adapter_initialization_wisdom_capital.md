# Adapter initialisieren: Wisdom Capital

Der folgende Code initialisiert [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new WisdomCapitalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	MarketDataKey = "<key>".ToSecureString(),
	MarketDataSecret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_wisdom_capital.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_wisdom_capital.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
