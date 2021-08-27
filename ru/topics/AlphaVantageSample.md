# Инициализация адаптера AlphaVantage

Код ниже демонстрирует как инициализировать [AlphaVantageMessageAdapter](xref:StockSharp.AlphaVantage.AlphaVantageMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

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
