# Adapter initialisieren: comdirect

Der folgende Code initialisiert [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_comdirect.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_comdirect.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
