# Инициализация адаптера: Bigul

Следующий код создаёт [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_bigul.md).

## См. также

[Настройки коннектора](configuration_bigul.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
