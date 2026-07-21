# Инициализация адаптера SynFutures

Код ниже демонстрирует как инициализировать [SynFuturesMessageAdapter](xref:StockSharp.SynFutures.SynFuturesMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SynFuturesMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
