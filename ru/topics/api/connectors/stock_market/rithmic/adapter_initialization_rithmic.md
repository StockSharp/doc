# Инициализация адаптера Rithmic

Код ниже демонстрирует как инициализировать [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    CertFile = "<Path to certificate file>",
    Server = RithmicServers.Real,
    //Server = RithmicServers.Test,
    //Server = RithmicServers.Simulator,  
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
