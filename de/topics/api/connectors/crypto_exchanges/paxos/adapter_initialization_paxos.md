# Initialisierung des Paxos-Adapters

Der folgende Code zeigt, wie man den [PaxosMessageAdapter](xref:StockSharp.Paxos.PaxosMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PaxosMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ihr Wert>".To<SecureString>(),
	ClientSecret = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
