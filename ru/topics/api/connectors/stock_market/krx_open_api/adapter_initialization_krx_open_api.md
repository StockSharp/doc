# Инициализация адаптера: KRX Open API

Следующий код создаёт [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KrxOpenApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_krx_open_api.md).

## См. также

[Настройки коннектора](configuration_krx_open_api.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
