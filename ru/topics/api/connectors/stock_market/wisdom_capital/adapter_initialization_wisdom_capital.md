# Инициализация адаптера: Wisdom Capital

Следующий код создаёт [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WisdomCapitalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	MarketDataKey = "<key>".ToSecureString(),
	MarketDataSecret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_wisdom_capital.md).

## См. также

[Настройки коннектора](configuration_wisdom_capital.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
