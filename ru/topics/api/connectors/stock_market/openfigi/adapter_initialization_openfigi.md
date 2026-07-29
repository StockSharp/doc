# Инициализация адаптера: OpenFIGI

Следующий код создаёт [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new OpenFigiMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_openfigi.md).

## См. также

[Настройки коннектора](configuration_openfigi.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
