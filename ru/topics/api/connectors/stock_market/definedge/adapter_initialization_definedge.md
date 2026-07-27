# Инициализация адаптера: Definedge

Следующий код создаёт [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_definedge.md).

## См. также

[Настройки коннектора](configuration_definedge.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
