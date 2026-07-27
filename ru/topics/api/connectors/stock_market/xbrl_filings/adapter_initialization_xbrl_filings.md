# Инициализация адаптера: XBRL Filings

Следующий код создаёт [XbrlFilingsMessageAdapter](xref:StockSharp.XbrlFilings.XbrlFilingsMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XbrlFilingsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_xbrl_filings.md).

## См. также

[Настройки коннектора](configuration_xbrl_filings.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
