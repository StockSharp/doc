# Candles

[S\#](../api.md) supports the following types of candles:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - a candle based on a time interval, timeframe. You can set both popular intervals (minutes, hours, daily) and customized ones. For example, 21 seconds, 4.5 minutes, etc.
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - a price range candle. A new candle is created when a trade appears with a price that exceeds the acceptable limits. The acceptable limit is formed each time based on the price of the first trade.
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - a candle is formed until the total volume of trades exceeds a specified limit. If a new trade exceeds the allowable volume, it is included in a new candle.
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - the same as [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), but the number of trades is used as a limitation instead of volume.
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - a point-and-figure chart candle (X-O chart).
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - Renko candle.

How to work with candles is shown in the SampleConnection example, which is located in the *Samples\/Common\/SampleConnection* folder.

The following images show [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) and [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) charts:

![sample timeframecandles](../../images/sample_timeframecandles.png)

![sample rangecandles](../../images/sample_rangecandles.png)

## Starting Data Retrieval

1. To get candles, create a subscription using the [Subscription](xref:StockSharp.BusinessEntities.Subscription) class:

```cs
// Create a subscription to 5-minute candles
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // Data type with timeframe specification
	security)  // Instrument
{
	// Configure additional parameters through the MarketData property
	MarketData = 
	{
		// Period for which we request historical data (last 30 days)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. To receive candles, subscribe to the [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) event, which signals the appearance of a new value for processing:

```cs
// Subscribe to the candle reception event
_connector.CandleReceived += OnCandleReceived;

// Candle reception event handler
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Here subscription is the subscription object we created
	// candle - the received candle
	
	// Check if the candle belongs to our subscription
	if (subscription == _candleSubscription)
	{
		// Draw the candle on the chart
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> The [Chart](xref:StockSharp.Xaml.Charting.Chart) graphical component is used to display candles.

3. Next, start the subscription through the [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)) method:

```cs
// Start the subscription
_connector.Subscribe(subscription);
```

After this, the [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) event will begin to be called.

4. The [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) event is called not only when a new candle appears but also when the current one changes.

If you need to display only **"complete"** candles, you need to check the [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) property of the received candle:

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check if the candle belongs to our subscription
	if (subscription != _candleSubscription)
		return;
	
	// Check if the candle is completed
	if (candle.State == CandleStates.Finished) 
	{
		// Create data for drawing
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// Draw the candle on the chart
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. Additional parameters can be configured for the subscription:

- **Candle building mode** - determines whether ready-made data will be requested or built from another data type:

```cs
// Request only ready-made data
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Only build from another data type
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Request ready-made data, and if not available - build
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **Source for building candles** - indicates from which data type to build candles if they are not directly available:

```cs
// Building candles from tick trades
subscription.MarketData.BuildFrom = DataType.Ticks;

// Building candles from order books
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Building candles from Level1
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **Field for building candles** - must be specified for certain data types:

```cs
// Building candles from the best bid price in Level1
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Building candles from the best ask price in Level1
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Building candles from the middle of the spread in the order book
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **Volume profile** - calculation of volume profile for candles:

```cs
// Enable volume profile calculation
subscription.MarketData.IsCalcVolumeProfile = true;
```

## Examples of Subscriptions to Different Candle Types

### Candles with Standard Timeframe

```cs
// 5-minute candles
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### Loading Only Historical Candles

```cs
// Loading only historical candles without transitioning to real-time
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // Specify end date
		BuildMode = MarketDataBuildModes.Load  // Only load ready-made data
	}
};
_connector.Subscribe(historicalSubscription);
```

### Building Non-Standard Timeframe Candles from Ticks

```cs
// Candles with a 21-second timeframe, built from ticks
var customTimeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromSeconds(21)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(customTimeFrameSubscription);
```

### Building Candles from Order Book Data

```cs
// Candles built from the middle of the spread in the order book
var depthBasedSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle
	}
};
_connector.Subscribe(depthBasedSubscription);
```

### Candles with Volume Profile

```cs
// 5-minute candles with volume profile calculation
var volumeProfileSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.LoadAndBuild,
		BuildFrom = DataType.Ticks,
		IsCalcVolumeProfile = true
	}
};
_connector.Subscribe(volumeProfileSubscription);
```

### Volume Candles

```cs
// Volume candles (each candle contains 1000 contracts in volume)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // Specify candle type and volume
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(volumeCandleSubscription);
```

### Tick Count Candles

```cs
// Tick count candles (each candle contains 1000 trades)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // Specify candle type and number of trades
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(tickCandleSubscription);
```

### Price Range Candles

```cs
// Price range candles with a range of 0.1 units
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // Specify candle type and price range
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(rangeCandleSubscription);
```

### Renko Candles

```cs
// Renko candles with a step of 0.1
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // Specify candle type and block size
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(renkoCandleSubscription);
```

### Point and Figure Candles (P&F)

```cs
// Point and Figure candles
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // Specify P&F parameters
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(pnfCandleSubscription);
```

## Next Steps

[Chart](candles/chart.md)

[Custom Candle Type](candles/custom_type_of_candle.md)