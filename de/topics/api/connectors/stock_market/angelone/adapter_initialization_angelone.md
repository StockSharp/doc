# Initialisierung des Angel One-Adapters

Der folgende Code zeigt, wie [AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	ApiKey = "<API-Schlüssel>".ToSecureString(),
	TotpSecret = "<TOTP-Geheimnis>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<Öffentliche Client-IP-Adresse>",
	MacAddress = "<MAC-Adresse>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
