# Инициализация адаптера QuickSwap

Код ниже демонстрирует как инициализировать [QuickSwapMessageAdapter](xref:StockSharp.QuickSwap.QuickSwapMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QuickSwapMessageAdapter(Connector.TransactionIdGenerator)
{
	GraphApiKey = "<Ваше значение>".To<SecureString>(),
	WalletAddress = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
