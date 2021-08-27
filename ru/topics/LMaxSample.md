# Инициализация адаптера LMAX

Код ниже демонстрирует как инициализировать [LmaxMessageAdapter](xref:StockSharp.LMAX.LmaxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new LmaxMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
