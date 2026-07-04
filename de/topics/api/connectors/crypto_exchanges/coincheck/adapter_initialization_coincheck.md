# Adapterinitialisierung Coincheck

Der folgende Code zeigt, wie der [CoincheckMessageAdapter](xref:StockSharp.Coincheck.CoincheckMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CoincheckMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
