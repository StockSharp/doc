# Adapter initialisieren: StocksTrader

Der folgende Code initialisiert [StocksTraderMessageAdapter](xref:StockSharp.StocksTrader.StocksTraderMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new StocksTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch das Token des gewählten Demo- oder Echtkontos.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
