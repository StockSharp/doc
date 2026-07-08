> [!WARNING]
> Diese Börse wurde im Juli 2017 nach einer Beschlagnahmung endgültig geschlossen. Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt als historische Referenz erhalten.

# WEX-(BTC-e)-Adapter initialisieren

Der folgende Code zeigt, wie der [BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

