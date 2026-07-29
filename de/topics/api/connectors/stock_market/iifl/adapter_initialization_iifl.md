# Adapter initialisieren: IIFL

Der folgende Code initialisiert [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
	ClientId = "<Ihre Clientkennung>",
	AuthorizationCode = "<Ihr Autorisierungscode>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_iifl.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_iifl.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
