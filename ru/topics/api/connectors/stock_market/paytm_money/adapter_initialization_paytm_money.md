# Инициализация адаптера: Paytm Money

Следующий код создаёт [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PaytmMoneyMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ReadAccessToken = "<token>".ToSecureString(),
	PublicAccessToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_paytm_money.md).

## См. также

[Настройки коннектора](configuration_paytm_money.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
