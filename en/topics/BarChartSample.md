# Adapter initialization BarChart

The code below demonstrates how to initialize the [BarChartMessageAdapter](../api/StockSharp.BarChart.BarChartMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new BarChartMessageAdapter(Connector.TransactionIdGenerator)
{
    Token \= "\<Your Token\>".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
