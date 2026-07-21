# Initialisierung des QFEX-Adapters

Der folgende Code zeigt, wie man den [QFEXMessageAdapter](xref:StockSharp.QFEX.QFEXMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QFEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr Wert>",
	Secret = "<Ihr Wert>".To<SecureString>(),
	AccountId = "<Ihr Wert>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
