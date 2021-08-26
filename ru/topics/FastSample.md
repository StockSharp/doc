# Инициализация адаптера FAST

Код ниже демонстрирует как инициализировать [FastMessageAdapter](../api/StockSharp.Fix.FastMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new FastMessageAdapter(Connector.TransactionIdGenerator)
{
    \/\/ задаем необходимый диалект
    Dialect \= typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
\/\/ загрузить настройки диалекта из конфиг файла биржи
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
