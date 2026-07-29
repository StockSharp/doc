# Adapter initialisieren: SSI

Der folgende Code initialisiert [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new SSIMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
	ClientId = "<Ihre Clientkennung>",
	PrivateKey = "<Ihr privater RSA-Schlüssel>".To<SecureString>(),
	Otp = "<Aktueller OTP-Code>".To<SecureString>(),
	Account = "<Ihre Kontonummer>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_ssi.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_ssi.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
