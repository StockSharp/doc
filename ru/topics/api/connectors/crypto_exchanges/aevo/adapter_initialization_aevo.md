# Инициализация адаптера Aevo

Код ниже демонстрирует как инициализировать [AevoMessageAdapter](xref:StockSharp.Aevo.AevoMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AevoMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>",
	ApiSecret = "<Ваше значение>".To<SecureString>(),
	WalletAddress = "<Ваше значение>",
	SigningKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
