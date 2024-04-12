# Инициализация адаптера Альфа\-Директ

Код ниже демонстрирует как инициализировать [AlfaDirectMessageAdapter](xref:StockSharp.AlfaDirect.AlfaDirectMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlfaDirectMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
