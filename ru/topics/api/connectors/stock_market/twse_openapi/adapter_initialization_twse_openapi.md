# Инициализация адаптера: TWSE

Следующий код создаёт [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TwseMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_twse_openapi.md).

## См. также

[Настройки коннектора](configuration_twse_openapi.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
