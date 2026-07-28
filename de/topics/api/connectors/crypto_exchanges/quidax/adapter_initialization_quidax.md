# Adapter initialisieren: Quidax

Der folgende Code initialisiert [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ihr Zugriffstoken>".To<SecureString>(),
	UserId = "<Ihre Benutzerkennung>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_quidax.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_quidax.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
