# Adapter initialisieren: InvertirOnline

Der folgende Code initialisiert [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new InvertirOnlineMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_invertironline.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_invertironline.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
