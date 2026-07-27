# Инициализация адаптера: Toss Securities

Следующий код создаёт [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TossSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_toss_securities.md).

## См. также

[Настройки коннектора](configuration_toss_securities.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
