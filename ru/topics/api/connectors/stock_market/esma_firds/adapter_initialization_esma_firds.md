# Инициализация адаптера: ESMA FIRDS

Следующий код создаёт [EsmaFirdsMessageAdapter](xref:StockSharp.EsmaFirds.EsmaFirdsMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EsmaFirdsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_esma_firds.md).

## См. также

[Настройки коннектора](configuration_esma_firds.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
