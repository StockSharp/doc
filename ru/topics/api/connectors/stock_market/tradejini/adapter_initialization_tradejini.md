# Инициализация адаптера: Tradejini

Следующий код создаёт [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradejiniMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<key>".ToSecureString(),
	Password = "<secret>".ToSecureString(),
	TwoFactorCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_tradejini.md).

## См. также

[Настройки коннектора](configuration_tradejini.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
