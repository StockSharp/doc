# Инициализация адаптера: Bavest

Следующий код создаёт [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BavestMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_bavest.md).

## См. также

[Настройки коннектора](configuration_bavest.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
