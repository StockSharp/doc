# Инициализация адаптера: StocksTrader

Следующий код создаёт [StocksTraderMessageAdapter](xref:StockSharp.StocksTrader.StocksTraderMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StocksTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените примеры токеном, выданным для выбранного демо- или реального счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
