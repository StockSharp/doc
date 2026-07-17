# Initialisierung des Longbridge OpenAPI-Adapters

Der folgende Code zeigt, wie [LongbridgeMessageAdapter](xref:StockSharp.Longbridge.LongbridgeMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new LongbridgeMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Anwendungsschlüssel>",
	AppSecret = "<Anwendungsgeheimnis>".ToSecureString(),
	AccessToken = "<Zugriffstoken>".ToSecureString(),
	Portfolio = "Longbridge",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
