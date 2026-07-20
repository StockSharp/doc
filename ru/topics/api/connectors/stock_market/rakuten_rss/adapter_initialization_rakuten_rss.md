# Инициализация адаптера Rakuten MARKETSPEED II RSS

Код ниже демонстрирует как инициализировать [RakutenRssMessageAdapter](xref:StockSharp.RakutenRss.RakutenRssMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RakutenRssMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
