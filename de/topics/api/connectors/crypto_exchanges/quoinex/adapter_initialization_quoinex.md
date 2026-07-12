> [!NOTE]
> QUOINEX wurde in Liquid umbenannt, das 2022 geschlossen wurde. Diese Dokumentation bleibt als historische Referenz erhalten.

# Quoinex-Adapter initialisieren

Der folgende Code zeigt, wie der [QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

