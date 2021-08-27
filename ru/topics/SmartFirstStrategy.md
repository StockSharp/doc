# Инициализация адаптера SmartCOM

Код ниже демонстрирует как инициализировать [SmartComMessageAdapter](xref:StockSharp.SmartCom.SmartComMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new SmartComMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Address = SmartComAddresses.Demo,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
