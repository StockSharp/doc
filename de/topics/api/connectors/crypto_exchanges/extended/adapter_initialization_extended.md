# Initialisierung des Extended-Adapters

Der folgende Code zeigt, wie man den [ExtendedMessageAdapter](xref:StockSharp.Extended.ExtendedMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ExtendedMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr Wert>".To<SecureString>(),
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
