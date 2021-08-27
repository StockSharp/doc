# Инициализация адаптера Interactive Brokers

Код ниже демонстрирует как инициализировать [InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Your Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
