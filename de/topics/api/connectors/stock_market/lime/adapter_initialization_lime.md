# Initialisierung des Lime Trader-Adapters

Der folgende Code zeigt, wie [LimeMessageAdapter](xref:StockSharp.Lime.LimeMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new LimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	ClientId = "<Clientkennung>",
	ClientSecret = "<Clientgeheimnis>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
