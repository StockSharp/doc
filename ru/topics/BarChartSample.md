# Инициализация адаптера BarChart

Код ниже демонстрирует как инициализировать [BarChartMessageAdapter](xref:StockSharp.BarChart.BarChartMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new BarChartMessageAdapter(Connector.TransactionIdGenerator)
{
    Token = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
