> [!CAUTION]
> **The FTX exchange has shut down. The connector no longer works; the documentation is retained for reference only.**

# Initialization of FTX Adapter

The code below demonstrates how to initialize the [FtxMessageAdapter](xref:StockSharp.FTX.FtxMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FtxMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>".To<SecureString>(),
	Secret = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
