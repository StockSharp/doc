# Initialisierung des Kotak Neo-Adapters

Der folgende Code zeigt, wie [KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<Consumer-Schlüssel>".ToSecureString(),
	MobileNumber = "<Mobilnummer>",
	UserCode = "<Benutzercode>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<TOTP-Geheimnis>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

