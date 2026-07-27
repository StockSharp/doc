# Инициализация адаптера: Zebu

Следующий код создаёт [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZebuMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	UserId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_zebu.md).

## См. также

[Настройки коннектора](configuration_zebu.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
