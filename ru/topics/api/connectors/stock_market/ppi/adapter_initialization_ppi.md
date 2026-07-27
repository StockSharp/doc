# Инициализация адаптера: PPI

Следующий код создаёт [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PpiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizedClient = "<id>",
	ClientKey = "<key>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_ppi.md).

## См. также

[Настройки коннектора](configuration_ppi.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
