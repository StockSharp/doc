# Initialisierung des DhanHQ-Adapters

Der folgende Code zeigt, wie [DhanMessageAdapter](xref:StockSharp.Dhan.DhanMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new DhanMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Clientkennung>",
	Token = "<Token>".ToSecureString(),
	DefaultProduct = DhanProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
