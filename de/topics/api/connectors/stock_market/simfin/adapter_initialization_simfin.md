# Adapter initialisieren: SimFin

Der folgende Code initialisiert [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new SimFinMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_simfin.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_simfin.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
