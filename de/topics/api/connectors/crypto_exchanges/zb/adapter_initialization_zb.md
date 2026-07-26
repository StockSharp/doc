> [!CAUTION]
> **Die Börse ZB und ihre API sind nicht mehr verfügbar. Der Konnektor funktioniert nicht; die Dokumentation dient nur noch als Referenz.**

# ZB-Adapter initialisieren

Der folgende Code zeigt, wie der [ZBMessageAdapter](xref:StockSharp.ZB.ZBMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new ZBMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
