# Adapter initialisieren: Nuvama

Der folgende Code initialisiert [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new NuvamaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestId = "<id>".ToSecureString(),
	AppIdKey = "<key>".ToSecureString(),
	PublicIpAddress = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_nuvama.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_nuvama.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
