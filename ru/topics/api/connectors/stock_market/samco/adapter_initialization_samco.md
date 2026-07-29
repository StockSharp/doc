# Инициализация адаптера: Samco

Следующий код создаёт [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SamcoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_samco.md).

## См. также

[Настройки коннектора](configuration_samco.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
