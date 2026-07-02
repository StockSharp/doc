> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (~2021 — geschlossen). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation wird zu historischen Referenzzwecken aufbewahrt.

# Adapterinitialisierung BW

Der folgende Code zeigt, wie man den [BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
