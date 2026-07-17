# Инициализация адаптера Questrade

В следующем коде показано, как инициализировать [QuestradeMessageAdapter](xref:StockSharp.Questrade.QuestradeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuestradeMessageAdapter(Connector.TransactionIdGenerator)
{
	RefreshToken = "<Токен обновления>".ToSecureString(),
	Account = "<Счёт>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере учётными данными и адресами серверов, выданными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
