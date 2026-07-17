# Initialisierung des NinjaTrader-Adapters

Der folgende Code zeigt, wie [NinjaTraderMessageAdapter](xref:StockSharp.NinjaTrader.NinjaTraderMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new NinjaTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	ClientId = "<Clientkennung>",
	Secret = "<Geheimnis>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = Guid.NewGuid().ToString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
