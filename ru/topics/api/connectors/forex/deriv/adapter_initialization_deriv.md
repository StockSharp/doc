# Инициализация адаптера: Deriv

Следующий код создаёт [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените примеры токеном и идентификатором приложения, выданными для выбранного демо- или реального счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
