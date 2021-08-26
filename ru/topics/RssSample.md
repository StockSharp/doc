# Инициализация адаптера RSS

Код ниже демонстрирует как инициализировать [RssMessageAdapter](../api/StockSharp.Rss.RssMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
    Address = new Uri("http://energy.rss"),
    CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
