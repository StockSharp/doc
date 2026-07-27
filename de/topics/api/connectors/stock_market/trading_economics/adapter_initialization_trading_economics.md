# Adapter initialisieren: Trading Economics

Der folgende Code initialisiert [TradingEconomicsMessageAdapter](xref:StockSharp.TradingEconomics.TradingEconomicsMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new TradingEconomicsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_trading_economics.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_trading_economics.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
