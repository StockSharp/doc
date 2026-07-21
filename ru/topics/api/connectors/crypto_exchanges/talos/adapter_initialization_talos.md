# Инициализация адаптера Talos

Код ниже демонстрирует как инициализировать [TalosMessageAdapter](xref:StockSharp.Talos.TalosMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TalosMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Ваше значение>".To<EndPoint>(),
	SenderCompId = "<Ваше значение>",
	TargetCompId = "<Ваше значение>",
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
