# Adapterinitialisierung J.P. Morgan DataQuery

Der folgende Code zeigt, wie der [JpmDataQueryMessageAdapter](xref:StockSharp.J.P. Morgan DataQuery.JpmDataQueryMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JpmDataQueryMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ihr Wert>",
	ClientSecret = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
