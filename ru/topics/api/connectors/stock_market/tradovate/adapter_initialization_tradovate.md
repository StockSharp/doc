# Инициализация адаптера Tradovate

Код ниже демонстрирует, как инициализировать [TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<имя пользователя>",
	Password = "<пароль>".ToSecureString(),
	ClientId = "<идентификатор клиента API>",
	Secret = "<секрет клиента API>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<постоянный идентификатор устройства>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Установите `IsDemo` в `false`, чтобы подключиться к реальной среде.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
