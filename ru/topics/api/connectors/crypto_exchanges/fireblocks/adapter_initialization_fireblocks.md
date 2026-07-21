# Инициализация адаптера Fireblocks

Код ниже демонстрирует как инициализировать [FireblocksMessageAdapter](xref:StockSharp.Fireblocks.FireblocksMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FireblocksMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
