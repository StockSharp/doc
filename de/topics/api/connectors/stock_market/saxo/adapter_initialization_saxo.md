# Initialisierung des Saxo OpenAPI-Adapters

Der folgende Code zeigt, wie [SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Zugriffstoken>".ToSecureString(),
	RefreshToken = "<Aktualisierungstoken>".ToSecureString(),
	ClientId = "<Clientkennung>",
	ClientSecret = "<Clientgeheimnis>".ToSecureString(),
	RedirectUri = "<Weiterleitungs-URI>",
	AccountKey = "<Kontoschlüssel>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
