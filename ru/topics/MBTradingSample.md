# Инициализация адаптера MB Trading

Код ниже демонстрирует как инициализировать [MBTradingMessageAdapter](../api/StockSharp.MBTrading.MBTradingMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
...	
var messageAdapter = new MBTradingMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
};
...	
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
