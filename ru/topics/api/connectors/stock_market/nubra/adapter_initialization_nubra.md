# Инициализация адаптера: Nubra

Следующий код создаёт [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NubraMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	DeviceId = "<id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_nubra.md).

## См. также

[Настройки коннектора](configuration_nubra.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
