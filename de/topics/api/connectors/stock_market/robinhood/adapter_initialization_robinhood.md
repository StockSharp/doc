# Initialisierung des Robinhood-Adapters

Der folgende Code zeigt, wie [RobinhoodMessageAdapter](xref:StockSharp.Robinhood.RobinhoodMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new RobinhoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	Address = new Uri("https://agent.robinhood.com/mcp/trading"),
	PollingInterval = TimeSpan.FromSeconds(2),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
