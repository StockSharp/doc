# Initialisierung des Bitunix-Adapters

Der folgende Code zeigt, wie man den [BitunixMessageAdapter](xref:StockSharp.Bitunix.BitunixMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitunixMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr Wert>".To<SecureString>(),
	Secret = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
