# Инициализация адаптера: TraderMade

Следующий код создаёт [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<Ваш ключ REST API>".To<SecureString>(),
	StreamingKey = "<Ваш ключ потокового API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_tradermade.md).

## См. также

[Настройки коннектора](configuration_tradermade.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
