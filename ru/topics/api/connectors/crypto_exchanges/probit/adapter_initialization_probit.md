# Инициализация адаптера ProBit Global

Код ниже показывает, как инициализировать [ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш идентификатор клиента OAuth>".To<SecureString>(),
	Secret = "<Ваш секрет клиента OAuth>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Если нужны только публичные рыночные данные, не задавайте `Key` и `Secret`.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
