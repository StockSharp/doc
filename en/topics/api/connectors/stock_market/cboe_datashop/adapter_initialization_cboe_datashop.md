# Adapter initialization Cboe DataShop / LiveVol

The code below demonstrates how to initialize the [CboeDataShopMessageAdapter](xref:StockSharp.CboeDataShop.CboeDataShopMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CboeDataShopMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your value>",
	Password = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
