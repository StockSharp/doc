# Инициализация адаптера: Capital.com

В следующем коде показано, как инициализировать [CapitalComMessageAdapter](xref:StockSharp.CapitalCom.CapitalComMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CapitalComMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<значение>".ToSecureString(),
	ApiKey = "<значение>",
	Login = "<значение>",
	AccountId = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
