# Инициализация адаптера FXCM

Код ниже демонстрирует как инициализировать [FxcmMessageAdapter](xref:StockSharp.Fxcm.FxcmMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FxcmMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Address = "<Your Address>".To<Uri>(),
    IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
