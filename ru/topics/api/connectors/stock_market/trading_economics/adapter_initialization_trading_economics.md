# Инициализация адаптера: Trading Economics

Следующий код создаёт [TradingEconomicsMessageAdapter](xref:StockSharp.TradingEconomics.TradingEconomicsMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradingEconomicsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_trading_economics.md).

## См. также

[Настройки коннектора](configuration_trading_economics.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
