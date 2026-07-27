# Инициализация адаптера: Mastertrust

Следующий код создаёт [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MastertrustMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<id>",
	OAuthClientSecret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_mastertrust.md).

## См. также

[Настройки коннектора](configuration_mastertrust.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
