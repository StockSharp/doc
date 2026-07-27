# Инициализация адаптера: EXANTE

Следующий код создаёт [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ExanteMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
	SummaryCurrency = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_exante.md).

## См. также

[Настройки коннектора](configuration_exante.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
