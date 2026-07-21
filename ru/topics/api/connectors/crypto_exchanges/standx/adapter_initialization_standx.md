# Инициализация адаптера StandX

Код ниже демонстрирует как инициализировать [StandXMessageAdapter](xref:StockSharp.StandX.StandXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new StandXMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
