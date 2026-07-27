# Adapter initialization: Trading Economics

The following code initializes [TradingEconomicsMessageAdapter](xref:StockSharp.TradingEconomics.TradingEconomicsMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradingEconomicsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_trading_economics.md) page.

## See also

[Connector configuration](configuration_trading_economics.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
