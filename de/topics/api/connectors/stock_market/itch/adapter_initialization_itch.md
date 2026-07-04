# Adapterinitialisierung ITCH

Der folgende Code zeigt, wie der [ItchMessageAdapter](xref:StockSharp.ITCH.ItchMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) uebergeben wird.

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new ItchMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
