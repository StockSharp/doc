# Инициализация адаптера: SET Market Data

Следующий код создаёт [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SetMarketDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_set_market_data.md).

## См. также

[Настройки коннектора](configuration_set_market_data.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
