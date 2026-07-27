# Инициализация адаптера: TPEx

Следующий код создаёт [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TpexMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_tpex.md).

## См. также

[Настройки коннектора](configuration_tpex.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
