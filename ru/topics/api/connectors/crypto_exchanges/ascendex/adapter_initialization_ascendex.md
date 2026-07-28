# Инициализация адаптера: AscendEX

Следующий код создаёт [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш API-ключ>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_ascendex.md).

## См. также

[Настройки коннектора](configuration_ascendex.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
