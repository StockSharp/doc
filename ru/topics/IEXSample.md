# Инициализация адаптера IEX

Код ниже демонстрирует как инициализировать [IEXMessageAdapter](xref:StockSharp.IEX.IEXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IEXMessageAdapter(Connector.TransactionIdGenerator)
{
    Token  = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
