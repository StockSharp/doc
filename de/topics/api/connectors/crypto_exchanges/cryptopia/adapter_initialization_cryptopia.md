> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (Mai 2019 - gehackt und liquidiert). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt zu historischen Referenzzwecken erhalten.

# Adapterinitialisierung Cryptopia

Der folgende Code zeigt, wie der [CryptopiaMessageAdapter](xref:StockSharp.Cryptopia.CryptopiaMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CryptopiaMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
