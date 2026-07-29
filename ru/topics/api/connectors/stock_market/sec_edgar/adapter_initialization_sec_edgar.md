# Инициализация адаптера: SEC EDGAR

Следующий код создаёт [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<Ваше приложение your-email@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_sec_edgar.md).

## См. также

[Настройки коннектора](configuration_sec_edgar.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
