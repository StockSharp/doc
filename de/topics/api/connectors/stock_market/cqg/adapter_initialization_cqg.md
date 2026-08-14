# Adapterinitialisierung CQG

Der folgende Code zeigt, wie [CqgMessageAdapter](xref:StockSharp.CQG.CqgMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new CqgMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	PrivateLabel = "WebAPITest",
	ClientId = "WebAPITest",
	Endpoint = "wss://demoapi.cqg.com:443",
	Portfolio = "<Portfolio>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
