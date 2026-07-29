# Adapter initialization: Finam Trade API

The following code initializes [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set `Token` to the Finam Trade API secret. Omit `AccountId` to let the adapter use the first account available to the token. Additional properties are described on the [Connector configuration](configuration_finam_trade.md) page.

## See also

[Connector configuration](configuration_finam_trade.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
