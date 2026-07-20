# Инициализация адаптера Mercado Bitcoin

Код ниже демонстрирует как инициализировать [MercadoBitcoinMessageAdapter](xref:StockSharp.MercadoBitcoin.MercadoBitcoinMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MercadoBitcoinMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваше значение>".To<SecureString>(),
	Secret = "<Ваше значение>".To<SecureString>(),
	AccountId = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
