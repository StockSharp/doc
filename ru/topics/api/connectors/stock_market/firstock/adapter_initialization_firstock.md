# Инициализация адаптера: Firstock

Следующий код создаёт [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FirstockMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	Password = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	VendorCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_firstock.md).

## См. также

[Настройки коннектора](configuration_firstock.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
