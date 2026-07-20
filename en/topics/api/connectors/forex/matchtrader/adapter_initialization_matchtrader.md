# Initialization of Match-Trader Adapter

The code below demonstrates how to initialize the [MatchTraderMessageAdapter](xref:StockSharp.MatchTrader.MatchTraderMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MatchTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your value>",
	Password = "<Your value>".To<SecureString>(),
	AccountId = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
