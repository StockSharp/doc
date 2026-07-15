# Инициализация адаптера TradeZero

Код ниже демонстрирует, как инициализировать [TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<необязательный маршрут заявки>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Свойство `DefaultRoute` можно не указывать, чтобы коннектор выбрал совместимый маршрут автоматически.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
