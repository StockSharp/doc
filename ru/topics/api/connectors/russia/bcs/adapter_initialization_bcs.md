# Инициализация адаптера: BCS

Следующий код создаёт [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BcsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_bcs.md).

## См. также

[Настройки коннектора](configuration_bcs.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
