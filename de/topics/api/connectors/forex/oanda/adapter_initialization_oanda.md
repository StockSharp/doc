# Oanda-Adapter initialisieren

Der folgende Code zeigt, wie der [OandaMessageAdapter](xref:StockSharp.Oanda.OandaMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OandaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
