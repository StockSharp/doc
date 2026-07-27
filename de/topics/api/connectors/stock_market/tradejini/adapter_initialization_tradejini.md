# Adapter initialisieren: Tradejini

Der folgende Code initialisiert [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new TradejiniMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<key>".ToSecureString(),
	Password = "<secret>".ToSecureString(),
	TwoFactorCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_tradejini.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_tradejini.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
