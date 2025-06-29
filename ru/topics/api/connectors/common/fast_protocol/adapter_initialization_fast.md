# Инициализация адаптера FAST

Код ниже демонстрирует, как инициализировать [FastMessageAdapter](xref:StockSharp.Fix.FastMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FastMessageAdapter(Connector.TransactionIdGenerator)
{
	// задаем необходимый диалект
	Dialect = typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
// загрузить настройки диалекта из конфиг файла биржи
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
