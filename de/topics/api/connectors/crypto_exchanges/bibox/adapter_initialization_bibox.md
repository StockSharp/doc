# Adapterinitialisierung Bibox

Der folgende Code zeigt, wie der [BiboxMessageAdapter](xref:StockSharp.Bibox.BiboxMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BiboxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
