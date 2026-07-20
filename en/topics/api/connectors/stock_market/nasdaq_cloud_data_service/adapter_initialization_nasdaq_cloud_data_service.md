# Adapter initialization Nasdaq Cloud Data Service

The code below demonstrates how to initialize the [NasdaqCloudDataServiceMessageAdapter](xref:StockSharp.NasdaqCloudDataService.NasdaqCloudDataServiceMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NasdaqCloudDataServiceMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your value>",
	Password = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
