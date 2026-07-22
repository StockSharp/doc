# Инициализация адаптера BitGo

Код ниже демонстрирует как инициализировать [BitGoMessageAdapter](xref:StockSharp.BitGo.BitGoMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitGoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ваше значение>".To<SecureString>(),
	Account = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
