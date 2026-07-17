# Инициализация адаптера: FXCM

В следующем коде показано, как инициализировать [FxcmMessageAdapter](xref:StockSharp.Fxcm.FxcmMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FxcmMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<значение>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
