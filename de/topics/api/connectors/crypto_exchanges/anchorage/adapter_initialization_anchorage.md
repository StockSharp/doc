# Initialisierung des Anchorage-Adapters

Der folgende Code zeigt, wie man den [AnchorageMessageAdapter](xref:StockSharp.Anchorage.AnchorageMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AnchorageMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr Wert>".To<SecureString>(),
	SigningKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
