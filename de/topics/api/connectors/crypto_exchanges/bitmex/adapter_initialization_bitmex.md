> [!CAUTION]
> **Die Börse BitMEX hat am 23. September 2026 ihren Betrieb eingestellt. Der Konnektor funktioniert nicht mehr; die Dokumentation dient nur noch als Referenz.**

# Adapterinitialisierung BitMEX

Der folgende Code zeigt, wie man den [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) sendet.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
