# Инициализация адаптера Micex (TEAP)

Код ниже демонстрирует как инициализировать [MicexMessageAdapter](xref:StockSharp.Micex.MicexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new MicexMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    Addresses = new[] { "<Address>".To<EndPoint>() },
	Server = "<Server>",
	CompressionLevel = CompressionLevels.None,
	Interface = MicexInterfaces.Stock18,
	OrderBookDepth = 20,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
