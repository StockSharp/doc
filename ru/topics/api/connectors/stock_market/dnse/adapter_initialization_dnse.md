# Инициализация адаптера: DNSE

Следующий код создаёт [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DnseMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	TradingToken = "<token>".ToSecureString(),
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_dnse.md).

## См. также

[Настройки коннектора](configuration_dnse.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
