# Инициализация адаптера IQFeed

Код ниже демонстрирует как инициализировать [IQFeedMessageAdapter](../api/StockSharp.IQFeed.IQFeedMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new IQFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Level1Address \= "127.0.0.1:5009".To\<EndPoint\>(),
	Level2Address \= "127.0.0.1:9200".To\<EndPoint\>(),
	LookupAddress \= "127.0.0.1:9100".To\<EndPoint\>(),
	AdminAddress \=  "127.0.0.1:9200".To\<EndPoint\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
