# Инициализация адаптера BarChart

Код ниже демонстрирует как инициализировать [BarChartMessageAdapter](../api/StockSharp.BarChart.BarChartMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new BarChartMessageAdapter(Connector.TransactionIdGenerator)
{
    Token \= "\<Your Token\>".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
