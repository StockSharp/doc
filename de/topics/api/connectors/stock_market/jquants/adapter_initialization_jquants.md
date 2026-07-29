# Adapter initialisieren: J-Quants

Der folgende Code initialisiert [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new JQuantsMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_jquants.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_jquants.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
