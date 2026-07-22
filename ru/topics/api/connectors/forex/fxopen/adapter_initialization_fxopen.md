# Инициализация адаптера: FXOpen TickTrader

Следующий код создаёт [FXOpenMessageAdapter](xref:StockSharp.FXOpen.FXOpenMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FXOpenMessageAdapter(Connector.TransactionIdGenerator)
{
	WebApiId = "<web-api-id>",
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените примеры параметрами токена для выбранного реального или демо-счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
