# Adapterinitialisierung AlphaVantage

Der folgende Code zeigt, wie der [AlphaVantageMessageAdapter](xref:StockSharp.AlphaVantage.AlphaVantageMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlphaVantageMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
