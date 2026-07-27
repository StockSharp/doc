# Инициализация адаптера: GuruFocus

Следующий код создаёт [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GuruFocusMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_gurufocus.md).

## См. также

[Настройки коннектора](configuration_gurufocus.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
