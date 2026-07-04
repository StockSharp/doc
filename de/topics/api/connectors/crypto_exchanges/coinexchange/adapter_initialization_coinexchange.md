> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (Oktober 2019 - Betrieb eingestellt). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt zu historischen Referenzzwecken erhalten.

# Adapterinitialisierung CoinExchange

Der folgende Code zeigt, wie der [CoinExchangeMessageAdapter](xref:StockSharp.CoinExchange.CoinExchangeMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CoinExchangeMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
