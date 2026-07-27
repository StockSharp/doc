# Инициализация адаптера: Rupeezy

Следующий код создаёт [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RupeezyMessageAdapter(Connector.TransactionIdGenerator)
{
	ApplicationId = "<id>",
	ApiKey = "<key>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_rupeezy.md).

## См. также

[Настройки коннектора](configuration_rupeezy.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
