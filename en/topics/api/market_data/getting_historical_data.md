# Getting historical data

[API](../../api.md) provides an opportunity to receive historical candles, which can be used both for testing and for building [Indicators](../indicators.md). 

## Working with historical candles through Connector

1. To get candles through [Connector](xref:StockSharp.Algo.Connector) , you need to create [Connector](xref:StockSharp.Algo.Connector) and add the corresponding [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) to it. How to do this using a special control, see [Connection settings window](../graphical_user_interface/connection_settings_window.md).

   You can also add the corresponding [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) to the [Connector](xref:StockSharp.Algo.Connector) through the code. For example, adapter initialization for [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) is described in [Adapter initialization Interactive Brokers](../connectors/stock_market/interactive_brokers/adapter_initialization_interactive_brokers.md) and looks like this:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
 	Address = "<Your Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
Connector.Connect();
...
							
```

2. In order to get historical candles, you need to call the [TraderHelper.SubscribeCandles](xref:StockSharp.Algo.TraderHelper.SubscribeCandles(StockSharp.Algo.ISubscriptionProvider,StockSharp.Algo.Candles.CandleSeries,System.Nullable{System.DateTimeOffset},System.Nullable{System.DateTimeOffset},System.Nullable{System.Int64},System.Nullable{System.Int64},StockSharp.Messages.IMessageAdapter,System.Nullable{System.Int64}))**(**[StockSharp.Algo.ISubscriptionProvider](xref:StockSharp.Algo.ISubscriptionProvider) provider, [StockSharp.Algo.Candles.CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) series, [System.Nullable\<System.DateTimeOffset\>](xref:System.Nullable`1) from, [System.Nullable\<System.DateTimeOffset\>](xref:System.Nullable`1) to, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) count, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId, [StockSharp.Messages.IMessageAdapter](xref:StockSharp.Messages.IMessageAdapter) adapter, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) skip **)** method: 

```cs
...
var tf = (TimeSpan)CandlesPeriods.SelectedItem;
var series = new CandleSeries(typeof(TimeFrameCandle), SelectedSecurity, tf);
Connector.SubscribeCandles(SelectedSecurity, DateTime.Now.Subtract(TimeSpan.FromTicks(tf.Ticks * 100)), DateTime.Now);
...
   			
```

3. Historical candles are passed through the [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing) event: 

```cs
...
Connector.CandleSeriesProcessing += ProcessCandle;
...
   			
```

4. Candles that appear can be rendered through the [Chart](../candles/chart.md).

## Working with historical candles through MessageAdapter

1. To get candles through the [MessageAdapter](xref:StockSharp.Messages.MessageAdapter), you need to create the corresponding [MessageAdapter](xref:StockSharp.Messages.MessageAdapter).

   For example, adapter initialization for [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) is described in [Adapter initialization Interactive Brokers](../connectors/stock_market/interactive_brokers/adapter_initialization_interactive_brokers.md) and looks like this:

```cs
   		
...         
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Your Address>".To<EndPoint>(),
};
...
   							
```

2. We wrap the [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) adapter in the [SecurityNativeIdMessageAdapter](xref:StockSharp.Algo.SecurityNativeIdMessageAdapter). security system identifier adapter. This is necessary if the trading system works with numeric or any other security identifiers other than the usual string representation.

```cs
   	
...

SecurityNativeIdMessageAdapter _securityAdapter;

if (adapter.IsNativeIdentifiers)
	_securityAdapter = new SecurityNativeIdMessageAdapter(adapter, new InMemoryNativeIdStorage());

var securities = _securityAdapter.GetSecurities(new SecurityLookupMessage
{
	SecurityId = new SecurityId
	{
		SecurityCode = "EUR"
	}
});

SecurityMessage eurUsd = null;

foreach (var security in securities)
{
	if (security.SecurityId.SecurityCode.CompareIgnoreCase("EURUSD"))
		eurUsd = security;
}
...
   							
```

3. Now, using the received security identifier, we get candles from the adapter: 

```cs
...
var candles = adapter.GetCandles(eurUsd.SecurityId, TimeSpan.FromDays(1), DateTimeOffset.Now.AddDays(-100), DateTimeOffset.Now);
foreach (var candle in candles)
{
	Console.WriteLine(candle);
}
...
   			
```

## Recommended content

[Candles](../candles.md)
