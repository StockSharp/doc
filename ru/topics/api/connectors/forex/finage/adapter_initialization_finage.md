# Инициализация адаптера: Finage

Следующий код создаёт [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new FinageMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Ваш ключ API>".To<SecureString>(),
	StreamingToken = "<Ваш потоковый токен>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_finage.md).

## См. также

[Настройки коннектора](configuration_finage.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
