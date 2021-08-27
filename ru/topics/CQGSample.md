# Инициализация адаптера CQG

Код ниже демонстрирует как инициализировать [CqgComMessageAdapter](xref:StockSharp.Cqg.Com.CqgComMessageAdapter) и [CqgContinuumMessageAdapter](xref:StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter) и передать их в [Connector](xref:StockSharp.Algo.Connector).

1. **CQG COM**, подключение через локальный **CQG Integrated Client**:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

2. **CQG Continuum**, подключение напрямую к серверу:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Address = "<Address>".To<IPAddress>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
