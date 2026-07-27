# Инициализация адаптера: Ventura

Следующий код создаёт [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_ventura.md).

## См. также

[Настройки коннектора](configuration_ventura.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
