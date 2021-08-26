# Инициализация адаптера Rithmic

Код ниже демонстрирует как инициализировать [RithmicMessageAdapter](../api/StockSharp.Rithmic.RithmicMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

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

[Окно настройки подключений](API_UI_ConnectorWindow.md)
