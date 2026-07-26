> [!CAUTION]
> **Die Börse FatBTC hat ihren Betrieb eingestellt. Der Konnektor funktioniert nicht mehr; die Dokumentation dient nur noch als Referenz.**

# Adapterinitialisierung FatBTC

Der folgende Code zeigt, wie der [FatBtcMessageAdapter](xref:StockSharp.FatBTC.FatBtcMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new FatBtcMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
