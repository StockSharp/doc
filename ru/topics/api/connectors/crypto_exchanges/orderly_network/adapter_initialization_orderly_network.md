# Инициализация адаптера Orderly Network

Код ниже демонстрирует как инициализировать [OrderlyNetworkMessageAdapter](xref:StockSharp.OrderlyNetwork.OrderlyNetworkMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OrderlyNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	AccountId = "<Ваше значение>",
	Secret = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
