# Инициализация адаптера: SimFin

Следующий код создаёт [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SimFinMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_simfin.md).

## См. также

[Настройки коннектора](configuration_simfin.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
