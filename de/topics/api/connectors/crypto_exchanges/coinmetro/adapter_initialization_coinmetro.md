# Adapter initialisieren: Coinmetro

Der folgende Code initialisiert [CoinmetroMessageAdapter](xref:StockSharp.Coinmetro.CoinmetroMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinmetroMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ihr Zugriffstoken>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_coinmetro.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_coinmetro.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
