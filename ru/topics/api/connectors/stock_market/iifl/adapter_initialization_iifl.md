# Инициализация адаптера: IIFL

Следующий код создаёт [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
	ClientId = "<Ваш идентификатор клиента>",
	AuthorizationCode = "<Ваш код авторизации>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_iifl.md).

## См. также

[Настройки коннектора](configuration_iifl.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
