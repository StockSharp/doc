# Инициализация адаптера: FINRA

Следующий код создаёт [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinraMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_finra.md).

## См. также

[Настройки коннектора](configuration_finra.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
