# Adapter initialisieren: Settrade

Der folgende Code initialisiert [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
	AppCode = "<Ihr Anwendungscode>",
	BrokerId = "<Ihre Brokerkennung>",
	Account = "<Ihre Kontonummer>",
	Pin = "<Ihre Handels-PIN>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie Zugangsdaten, Kontotyp und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_settrade.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_settrade.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
