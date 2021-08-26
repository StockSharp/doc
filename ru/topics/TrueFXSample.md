# Инициализация адаптера TrueFX

Код ниже демонстрирует как инициализировать [TrueFXMessageAdapter](../api/StockSharp.TrueFX.TrueFXMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
...	
var messageAdapter \= new TrueFXMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
};
...	
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
