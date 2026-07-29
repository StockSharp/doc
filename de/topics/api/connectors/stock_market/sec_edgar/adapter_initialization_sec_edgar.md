# Adapter initialisieren: SEC EDGAR

Der folgende Code initialisiert [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<Ihre Anwendung ihre-email@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_sec_edgar.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_sec_edgar.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
