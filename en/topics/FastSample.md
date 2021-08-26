# Adapter initialization FAST

The code below demonstrates how to initialize the [FastMessageAdapter](../api/StockSharp.Fix.FastMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FastMessageAdapter(Connector.TransactionIdGenerator)
{
    // choose required dialect
    Dialect = typeof(StockSharp.Fix.Dialects.Bovespa.BovespaFastDialect),
};
// load all dialect settings from an exchange config file
messageAdapter.DialectSettings.LoadSettingsFromFile(configFile);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
