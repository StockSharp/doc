# Инициализация адаптера Charles Schwab

Код ниже демонстрирует, как инициализировать [SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<токен доступа>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
