# Инициализация адаптера Webull

Код ниже демонстрирует, как инициализировать [WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<ключ приложения>".ToSecureString(),
	Secret = "<секрет приложения>".ToSecureString(),
	Token = "<токен доступа>".ToSecureString(),
	Account = "<идентификатор счета>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Параметры `Token` и `Account` можно не задавать, если они не требуются.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
