# Инициализация адаптера Tardis

Код ниже демонстрирует как инициализировать [TardisMessageAdapter](xref:StockSharp.Tardis.TardisMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TardisMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ваше значение>".To<SecureString>(),
	Exchange = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
