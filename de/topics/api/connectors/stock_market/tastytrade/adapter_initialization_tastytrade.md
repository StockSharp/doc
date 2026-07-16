# Initialisierung des tastytrade-Adapters

Der folgende Code zeigt, wie [TastyTradeMessageAdapter](xref:StockSharp.TastyTrade.TastyTradeMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new TastyTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	ClientSecret = "<Clientgeheimnis>".ToSecureString(),
	Scopes = TastyTradeScopes.Read | TastyTradeScopes.Trade,
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

