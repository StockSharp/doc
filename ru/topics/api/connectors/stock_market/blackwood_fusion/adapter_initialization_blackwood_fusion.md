# Инициализация адаптера Blackwood (Fusion)

Код ниже демонстрирует как инициализировать [BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var address = "<Address>".To<IPAddress>();
var messageAdapter = new BlackwoodMessageAdapter(Connector.TransactionIdGenerator)
{
    Login = "<Your Login>",
    Password = "<Your Password>".To<SecureString>(),
    ExecutionAddress = new IPEndPoint(address, BlackwoodAddresses.ExecutionPort),
    MarketDataAddress = new IPEndPoint(address, BlackwoodAddresses.MarketDataPort),
    HistoricalDataAddress = new IPEndPoint(address, BlackwoodAddresses.HistoricalDataPort)
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
