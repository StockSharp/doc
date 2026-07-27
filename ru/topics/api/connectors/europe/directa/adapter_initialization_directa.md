# Инициализация адаптера: Directa

Следующий код создаёт [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DirectaMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_directa.md).

## См. также

[Настройки коннектора](configuration_directa.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
