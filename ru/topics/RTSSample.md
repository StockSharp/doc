# Инициализация адаптера РТС

Код ниже демонстрирует как инициализировать [SpbExMessageAdapter](../api/StockSharp.SpbEx.SpbExMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new RtsHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
