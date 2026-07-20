# Инициализация адаптера J.P. Morgan DataQuery

Код ниже демонстрирует как инициализировать [JpmDataQueryMessageAdapter](xref:StockSharp.J.P. Morgan DataQuery.JpmDataQueryMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JpmDataQueryMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ваше значение>",
	ClientSecret = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
