# Adapter initialization: StocksTrader

The following code initializes [StocksTraderMessageAdapter](xref:StockSharp.StocksTrader.StocksTraderMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StocksTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the token issued for the selected demo or real account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
