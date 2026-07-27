# Инициализация адаптера: Unusual Whales

Следующий код создаёт [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UnusualWhalesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_unusual_whales.md).

## См. также

[Настройки коннектора](configuration_unusual_whales.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
