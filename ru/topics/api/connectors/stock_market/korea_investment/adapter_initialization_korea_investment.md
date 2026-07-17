# Инициализация адаптера: Korea Investment & Securities

В следующем коде показано, как инициализировать [KoreaInvestmentMessageAdapter](xref:StockSharp.KoreaInvestment.KoreaInvestmentMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KoreaInvestmentMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<значение>".ToSecureString(),
	AppSecret = "<значение>".ToSecureString(),
	AccountNumber = "<значение>",
	ProductCode = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
