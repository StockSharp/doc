# Инициализация адаптера: LS Securities

В следующем коде показано, как инициализировать [LsSecuritiesMessageAdapter](xref:StockSharp.LsSecurities.LsSecuritiesMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LsSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<значение>".ToSecureString(),
	AppSecret = "<значение>".ToSecureString(),
	Account = "<значение>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
