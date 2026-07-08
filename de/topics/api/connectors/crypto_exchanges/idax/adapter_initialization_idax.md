> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (November 2019 - geschlossen). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt als historische Referenz erhalten.

# Adapterinitialisierung Idax

Der folgende Code zeigt, wie der [IdaxMessageAdapter](xref:StockSharp.Idax.IdaxMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new IdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
