# Инициализация адаптера Interactive Brokers

Код ниже демонстрирует как инициализировать [InteractiveBrokersMessageAdapter](../api/StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address \= "\<Your Address\>".To\<EndPoint\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
