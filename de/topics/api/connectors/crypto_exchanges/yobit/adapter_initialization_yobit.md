> [!WARNING]
> Diese Börse ist seit etwa 2023 faktisch nicht mehr aktiv. Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt als historische Referenz erhalten.

# Yobit-Adapter initialisieren

Der folgende Code zeigt, wie der [YobitMessageAdapter](xref:StockSharp.Yobit.YobitMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new YobitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

