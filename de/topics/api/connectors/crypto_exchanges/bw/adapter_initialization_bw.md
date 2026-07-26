> [!CAUTION]
> **Die Börse BW hat ihren Betrieb eingestellt. Der Konnektor funktioniert nicht mehr; die Dokumentation dient nur noch als Referenz.**

# Adapterinitialisierung BW

Der folgende Code zeigt, wie man den [BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
