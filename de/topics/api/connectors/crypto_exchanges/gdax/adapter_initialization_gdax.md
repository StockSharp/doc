> [!CAUTION]
> **Der Dienst GDAX ist nicht mehr verfügbar. Auch sein Nachfolger Coinbase Pro wurde eingestellt; für Coinbase sollte der aktuelle Coinbase-Konnektor verwendet werden. Dieser Konnektor funktioniert nicht; die Dokumentation dient nur noch als Referenz.**

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
