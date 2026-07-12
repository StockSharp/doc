> [!NOTE]
> GDAX wurde in Coinbase Pro und später in Coinbase Advanced Trade umbenannt. Diese Dokumentation bleibt als historische Referenz erhalten.

# Adapterinitialisierung GDAX

Der folgende Code zeigt, wie der [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ihr API-Schlüssel>".To<SecureString>(),
				Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
