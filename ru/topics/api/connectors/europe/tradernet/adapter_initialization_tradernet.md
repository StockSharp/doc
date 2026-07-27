# Инициализация адаптера: Tradernet

Следующий код создаёт [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradernetMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_tradernet.md).

## См. также

[Настройки коннектора](configuration_tradernet.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
