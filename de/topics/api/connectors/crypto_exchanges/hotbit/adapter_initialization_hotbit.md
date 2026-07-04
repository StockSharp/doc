> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (Mai 2023 - geschlossen). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt als historische Referenz erhalten.

# Adapterinitialisierung Hotbit

Der folgende Code zeigt, wie der [HotbitMessageAdapter](xref:StockSharp.Hotbit.HotbitMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HotbitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
