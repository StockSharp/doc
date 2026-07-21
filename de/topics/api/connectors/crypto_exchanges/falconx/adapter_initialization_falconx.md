# Initialisierung des FalconX-Adapters

Der folgende Code zeigt, wie man den [FalconXMessageAdapter](xref:StockSharp.FalconX.FalconXMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FalconXMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr Wert>",
	Secret = "<Ihr Wert>".To<SecureString>(),
	Passphrase = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
