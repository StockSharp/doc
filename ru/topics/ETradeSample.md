# Инициализация адаптера E\*TRADE

Код ниже демонстрирует как инициализировать [ETradeMessageAdapter](../api/StockSharp.ETrade.ETradeMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new ETradeMessageAdapter(Connector.TransactionIdGenerator)
{
    ConsumerSecret = "<Your Secret>".To<SecureString>(),
    ConsumerKey = "<Your Key>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
