# Initialisierung des Pyth-Adapters

Der folgende Code zeigt, wie man den [PythMessageAdapter](xref:StockSharp.Pyth.PythMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PythMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
