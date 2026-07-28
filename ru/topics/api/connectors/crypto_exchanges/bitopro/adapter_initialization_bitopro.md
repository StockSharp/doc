# Инициализация адаптера: BitoPro

Следующий код создаёт [BitoProMessageAdapter](xref:StockSharp.BitoPro.BitoProMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BitoProMessageAdapter(connector.TransactionIdGenerator)
{
	Email = "<Ваша электронная почта>",
	Key = "<Ваш API-ключ>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_bitopro.md).

## См. также

[Настройки коннектора](configuration_bitopro.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
