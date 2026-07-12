# Adapterinitialisierung Ligther

Der folgende Code zeigt, wie [LigtherMessageAdapter](xref:StockSharp.Ligther.LigtherMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new LigtherMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
	AccountIndex = 0,
	ApiKeyIndex = 0,
	Section = LigtherSections.Derivatives,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
