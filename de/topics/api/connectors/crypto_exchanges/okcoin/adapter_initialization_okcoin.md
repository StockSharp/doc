> [!CAUTION]
> **Die Handelsdienste von OKCoin wurden im Zuge der Umstellung der Plattform auf OKX abgeschaltet. Dieser Konnektor funktioniert nicht mehr; die Dokumentation dient nur noch als Referenz.**

# OKCoin-Adapter initialisieren

Der folgende Code zeigt, wie der [OkcoinMessageAdapter](xref:StockSharp.Okcoin.OkcoinMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new OkcoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

