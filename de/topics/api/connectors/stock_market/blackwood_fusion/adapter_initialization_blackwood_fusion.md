# Adapterinitialisierung Blackwood (Fusion)

Der folgende Code zeigt, wie der [BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

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

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
