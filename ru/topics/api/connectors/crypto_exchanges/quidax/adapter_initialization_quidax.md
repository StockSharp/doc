# Инициализация адаптера: Quidax

Следующий код создаёт [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ваш токен доступа>".To<SecureString>(),
	UserId = "<Идентификатор пользователя>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_quidax.md).

## См. также

[Настройки коннектора](configuration_quidax.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
