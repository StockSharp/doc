# Adapter initialisieren: StockData.org

Der folgende Code initialisiert [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new StockDataOrgMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_stockdata_org.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_stockdata_org.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
