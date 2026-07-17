# Инициализация адаптера: Groww

В следующем коде показано, как инициализировать [GrowwMessageAdapter](xref:StockSharp.Groww.GrowwMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GrowwMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<значение>".ToSecureString(),
	ApiKey = "<значение>".ToSecureString(),
	ApiSecret = "<значение>".ToSecureString(),
	TotpSecret = "<значение>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
