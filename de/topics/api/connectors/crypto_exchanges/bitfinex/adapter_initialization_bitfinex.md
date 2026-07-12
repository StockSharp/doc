# Adapterinitialisierung Bitfinex

Der folgende Code zeigt, wie man den [BitfinexMessageAdapter](xref:StockSharp.Bitfinex.BitfinexMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) sendet.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitfinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
