# Инициализация адаптера Bit2Me

Код ниже демонстрирует инициализацию [Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter) и передачу его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш API-ключ>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Если нужны только публичные рыночные данные, не задавайте `Key` и `Secret`. Адреса REST и WebSocket можно изменить через `RestEndpoint` и `WebSocketEndpoint`.

## Рекомендуемые материалы

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
