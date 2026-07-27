# Инициализация адаптера: comdirect

Следующий код создаёт [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_comdirect.md).

## См. также

[Настройки коннектора](configuration_comdirect.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
