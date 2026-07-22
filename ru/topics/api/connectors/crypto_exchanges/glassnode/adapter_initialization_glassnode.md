# Инициализация адаптера Glassnode

Код ниже демонстрирует как инициализировать [GlassnodeMessageAdapter](xref:StockSharp.Glassnode.GlassnodeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GlassnodeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
