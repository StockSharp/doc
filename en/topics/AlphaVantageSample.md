# Adapter initialization AlphaVantage

The code below demonstrates how to initialize the [AlphaVantageMessageAdapter](xref:StockSharp.AlphaVantage.AlphaVantageMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlphaVantageMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
          
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
