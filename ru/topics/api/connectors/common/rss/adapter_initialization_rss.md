# Инициализация адаптера RSS

Код ниже демонстрирует, как инициализировать [RssMessageAdapter](xref:StockSharp.Rss.RssMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new Uri("http://energy.rss"),
	CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
