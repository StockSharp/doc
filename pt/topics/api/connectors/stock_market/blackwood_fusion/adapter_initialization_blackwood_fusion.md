# Inicialização do adaptador Blackwood (Fusion)

O código abaixo demonstra como inicializar o [BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

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

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
