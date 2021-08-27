# Инициализация адаптера TrueFX

Код ниже демонстрирует как инициализировать [TrueFXMessageAdapter](xref:StockSharp.TrueFX.TrueFXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
...	
var messageAdapter = new TrueFXMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
};
...	
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
