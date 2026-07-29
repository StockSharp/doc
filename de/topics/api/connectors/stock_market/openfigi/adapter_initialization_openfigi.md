# Adapter initialisieren: OpenFIGI

Der folgende Code initialisiert [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new OpenFigiMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_openfigi.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_openfigi.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
