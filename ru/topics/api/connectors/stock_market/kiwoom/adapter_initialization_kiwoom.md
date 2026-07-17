# Инициализация адаптера: Kiwoom

В следующем коде показано, как инициализировать [KiwoomMessageAdapter](xref:StockSharp.Kiwoom.KiwoomMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KiwoomMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<значение>".ToSecureString(),
	AppSecret = "<значение>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
