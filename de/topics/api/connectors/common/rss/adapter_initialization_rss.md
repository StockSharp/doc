# Adapterinitialisierung RSS

Der folgende Code zeigt, wie der [RssMessageAdapter](xref:StockSharp.Rss.RssMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new Uri("http://energy.rss"),
	CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
