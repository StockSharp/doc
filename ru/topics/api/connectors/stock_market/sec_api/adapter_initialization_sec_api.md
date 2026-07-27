# Инициализация адаптера: SEC API

Следующий код создаёт [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SecApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_sec_api.md).

## См. также

[Настройки коннектора](configuration_sec_api.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
