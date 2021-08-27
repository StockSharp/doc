# Инициализация адаптера OpenECry

Код ниже демонстрирует как инициализировать [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OpenECryMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Address = "<Address>".To<EndPoint>(),
    EnableOECLogging = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
