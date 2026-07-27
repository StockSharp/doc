# Adapter initialisieren: Marketaux

Der folgende Code initialisiert [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new MarketauxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_marketaux.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_marketaux.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
