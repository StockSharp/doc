# Initialisierung des Charles-Schwab-Adapters

Der folgende Code zeigt, wie der [SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Zugriffstoken>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
