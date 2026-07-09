# Adapterinitialisierung IQFeed

Der folgende Code zeigt, wie der [IQFeedMessageAdapter](xref:StockSharp.IQFeed.IQFeedMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new IQFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Level1Address = "127.0.0.1:5009".To<EndPoint>(),
	Level2Address = "127.0.0.1:9200".To<EndPoint>(),
	LookupAddress = "127.0.0.1:9100".To<EndPoint>(),
	AdminAddress =  "127.0.0.1:9200".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
