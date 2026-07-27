# Инициализация адаптера: Jainam

Следующий код создаёт [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JainamMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	AppCode = "<id>",
	ApiSecret = "<secret>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_jainam.md).

## См. также

[Настройки коннектора](configuration_jainam.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
