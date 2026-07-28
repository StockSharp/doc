# Adapter initialisieren: XRPL DEX

Der folgende Code initialisiert [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<Ihre XRPL-Kontoadresse>",
	Seed = "<Ihr geheimer Familienschlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Kontozugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_xrpl.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_xrpl.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
