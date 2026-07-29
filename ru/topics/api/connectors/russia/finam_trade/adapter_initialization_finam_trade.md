# Инициализация адаптера: Finam Trade API

Следующий код создаёт [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

В `Token` укажите секрет Finam Trade API. Не задавайте `AccountId`, если адаптер должен использовать первый доступный токену счёт. Дополнительные свойства описаны на странице [Настройки коннектора](configuration_finam_trade.md).

## См. также

[Настройки коннектора](configuration_finam_trade.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
