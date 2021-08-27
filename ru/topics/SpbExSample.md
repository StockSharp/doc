# Инициализация адаптера SPB Exchange

Код ниже демонстрирует как инициализировать [SpbExMessageAdapter](xref:StockSharp.SpbEx.SpbExMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new SpbExMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Config = SpbExAddressConfig.Game,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
