# Adapter initialisieren: EDINET

Der folgende Code initialisiert [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new EdinetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_edinet.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_edinet.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
