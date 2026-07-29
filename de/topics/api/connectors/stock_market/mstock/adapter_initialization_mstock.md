# Adapter initialisieren: m.Stock

Der folgende Code initialisiert [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	ClientCode = "<Ihr Clientcode>",
	Password = "<Ihr Passwort>".To<SecureString>(),
	Otp = "<Aktueller OTP-Code>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_mstock.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_mstock.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
