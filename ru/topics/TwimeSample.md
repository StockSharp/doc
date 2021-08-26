# Инициализация адаптера Twime

Код ниже демонстрирует как инициализировать [TwimeMessageAdapter](../api/StockSharp.Twime.TwimeMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new TwimeMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
