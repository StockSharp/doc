# Adapter initialisieren: Delta Exchange India

Der folgende Code initialisiert [DeltaExchangeIndiaMessageAdapter](xref:StockSharp.DeltaExchangeIndia.DeltaExchangeIndiaMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new DeltaExchangeIndiaMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_delta_exchange_india.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_delta_exchange_india.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
