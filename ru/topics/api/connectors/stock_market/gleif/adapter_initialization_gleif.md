# Инициализация адаптера: GLEIF

Следующий код создаёт [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GleifMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_gleif.md).

## См. также

[Настройки коннектора](configuration_gleif.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
