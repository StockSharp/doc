# Adapter-Initialisierung QuantFEED

Der folgende Code zeigt, wie der [QuantFeedMessageAdapter](xref:StockSharp.QuantHouse.QuantFeedMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new QuantFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
