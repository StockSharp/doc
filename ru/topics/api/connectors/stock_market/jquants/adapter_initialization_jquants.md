# Инициализация адаптера: J-Quants

Следующий код создаёт [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new JQuantsMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_jquants.md).

## См. также

[Настройки коннектора](configuration_jquants.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
