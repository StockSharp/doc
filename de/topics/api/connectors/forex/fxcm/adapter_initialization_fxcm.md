# FXCM-Adapter initialisieren

Der folgende Code zeigt, wie der [FxcmMessageAdapter](xref:StockSharp.Fxcm.FxcmMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FxcmMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Your Address>".To<Uri>(),
	IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
