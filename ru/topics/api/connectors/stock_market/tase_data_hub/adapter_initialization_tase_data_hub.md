# Инициализация адаптера: TASE Data Hub

Следующий код создаёт [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TaseDataHubMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_tase_data_hub.md).

## См. также

[Настройки коннектора](configuration_tase_data_hub.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
