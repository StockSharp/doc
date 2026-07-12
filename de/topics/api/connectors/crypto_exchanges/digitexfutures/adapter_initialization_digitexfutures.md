> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (ca. 2022 - Betrieb eingestellt). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation bleibt zu historischen Referenzzwecken erhalten.

# Adapterinitialisierung DigitexFutures

Der folgende Code zeigt, wie der [DigitexFuturesMessageAdapter](xref:StockSharp.DigitexFutures.DigitexFuturesMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new DigitexFuturesMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
