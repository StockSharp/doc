# Initialisierung des Kaiko-Adapters

Der folgende Code zeigt, wie man den [KaikoMessageAdapter](xref:StockSharp.Kaiko.KaikoMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new KaikoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
