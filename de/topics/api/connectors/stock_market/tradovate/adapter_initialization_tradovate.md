# Initialisierung des Tradovate-Adapters

Der folgende Code zeigt, wie [TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	ClientId = "<API-Clientkennung>",
	Secret = "<API-Clientgeheimnis>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<dauerhafte Gerätekennung>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Setzen Sie `IsDemo` auf `false`, um eine Verbindung mit der Live-Umgebung herzustellen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
