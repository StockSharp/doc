# Инициализация адаптера Dow Jones

Код ниже демонстрирует как инициализировать [DowJonesMessageAdapter](xref:StockSharp.DowJones.DowJonesMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DowJonesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ваше значение>".To<SecureString>(),
	ClientId = "<Ваше значение>",
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
