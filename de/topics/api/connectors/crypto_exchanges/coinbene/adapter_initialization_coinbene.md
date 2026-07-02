> [!WARNING]
> Diese Börse wurde dauerhaft geschlossen (~2021 — geschlossen). Dieser Connector ist nicht mehr funktionsfähig. Die Dokumentation wird zu historischen Referenzzwecken aufbewahrt.

# Adapterinitialisierung CoinBene

Der folgende Code zeigt, wie der [CoinBeneMessageAdapter](xref:StockSharp.CoinBene.CoinBeneMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) gesendet wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new CoinBeneMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlener Inhalt

[Fenster der Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
