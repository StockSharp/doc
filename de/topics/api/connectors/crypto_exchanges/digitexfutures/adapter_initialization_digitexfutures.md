> [!CAUTION]
> **Die Börse Digitex Futures hat ihren Betrieb eingestellt. Der Konnektor funktioniert nicht mehr; die Dokumentation dient nur noch als Referenz.**

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
