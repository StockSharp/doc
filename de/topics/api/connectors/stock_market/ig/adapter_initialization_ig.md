# Initialisierung des IG Markets-Adapters

Der folgende Code zeigt, wie [IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<API-Schlüssel>",
	UserName = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	AccountId = "<Kontokennung>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

