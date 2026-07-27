# Инициализация адаптера: Nuvama

Следующий код создаёт [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new NuvamaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestId = "<id>".ToSecureString(),
	AppIdKey = "<key>".ToSecureString(),
	PublicIpAddress = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_nuvama.md).

## См. также

[Настройки коннектора](configuration_nuvama.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
