# Инициализация адаптера: DEX Screener

Следующий код создаёт [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexScreenerMessageAdapter(connector.TransactionIdGenerator);
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_dex_screener.md).

## См. также

[Настройки коннектора](configuration_dex_screener.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
