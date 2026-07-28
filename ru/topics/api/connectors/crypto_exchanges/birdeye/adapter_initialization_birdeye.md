# Инициализация адаптера: Birdeye

Следующий код создаёт [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BirdeyeMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ваш токен доступа>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_birdeye.md).

## См. также

[Настройки коннектора](configuration_birdeye.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
