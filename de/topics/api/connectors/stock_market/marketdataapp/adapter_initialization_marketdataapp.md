# Adapter initialisieren: MarketData.app

Der folgende Code initialisiert [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new MarketDataAppMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ihr API-Token>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_marketdataapp.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_marketdataapp.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
