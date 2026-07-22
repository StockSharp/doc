# Инициализация адаптера Zero Hash

Код ниже демонстрирует как инициализировать [ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>",
	Secret = "<Ваше значение>".To<SecureString>(),
	Passphrase = "<Ваше значение>".To<SecureString>(),
	Account = "<Ваше значение>",
	User = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
