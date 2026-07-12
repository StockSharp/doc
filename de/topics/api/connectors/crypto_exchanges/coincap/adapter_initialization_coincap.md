# Adapterinitialisierung CoinCap

Der folgende Code zeigt, wie der [CoinCapMessageAdapter](xref:StockSharp.CoinCap.CoinCapMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) gesendet wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CoinCapMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster der Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
