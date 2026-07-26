> [!CAUTION]
> **Die Börse CoinHub und ihre API sind nicht mehr verfügbar. Der Konnektor funktioniert nicht; die Dokumentation dient nur noch als Referenz.**

# Adapterinitialisierung CoinHub

Der folgende Code zeigt, wie der [CoinHubMessageAdapter](xref:StockSharp.CoinHub.CoinHubMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CoinHubMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
