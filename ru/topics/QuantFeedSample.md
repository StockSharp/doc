# Инициализация адаптера QuantFEED

Код ниже демонстрирует как инициализировать [QuantFeedMessageAdapter](../api/StockSharp.QuantHouse.QuantFeedMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new QuantFeedMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
    Address \= "\<Address\>".To\<EndPoint\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
