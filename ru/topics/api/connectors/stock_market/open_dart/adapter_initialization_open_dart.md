# Инициализация адаптера: Open DART

Следующий код создаёт [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenDartMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_open_dart.md).

## См. также

[Настройки коннектора](configuration_open_dart.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
