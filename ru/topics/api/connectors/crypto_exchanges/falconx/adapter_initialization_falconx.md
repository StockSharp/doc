# Инициализация адаптера FalconX

Код ниже демонстрирует как инициализировать [FalconXMessageAdapter](xref:StockSharp.FalconX.FalconXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FalconXMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>",
	Secret = "<Ваше значение>".To<SecureString>(),
	Passphrase = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
