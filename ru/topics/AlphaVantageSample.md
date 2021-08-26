# Инициализация адаптера AlphaVantage

Код ниже демонстрирует как инициализировать [AlphaVantageMessageAdapter](../api/StockSharp.AlphaVantage.AlphaVantageMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlphaVantageMessageAdapter(Connector.TransactionIdGenerator)
{
    Token = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
