# Custom Type of Candle

[S\#](../../api.md) allows you to extend candle building capabilities by providing the ability to work with custom candle types. This is useful in cases where you need to work with candles that are not currently supported by [S\#](../../api.md). Below is the process of creating your own candle type using the example of Delta-candles (candles formed based on the difference between buy and sell volumes).

## Implementing Delta-candles

1. First, you need to create your own candle message type. The type must inherit from the [CandleMessage](xref:StockSharp.Messages.CandleMessage) class:

   ```cs
   /// <summary>
   /// Candle formed based on the delta of buy and sell volumes.
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
      // We get the message type identifier from the helper
      // to use the same value in RegisterCandleMessageType
       
       /// <summary>
       /// Initialize a new instance of <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }
       
       /// <summary>
       /// Delta threshold value for candle formation.
       /// </summary>
       public decimal DeltaThreshold { get; set; }
       
       /// <summary>
       /// Current delta value.
       /// </summary>
       public decimal CurrentDelta { get; set; }
       
       /// <summary>
       /// Create a copy of <see cref="DeltaCandleMessage"/>.
       /// </summary>
       /// <returns>Copy.</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }
       
       /// <summary>
       /// Candle parameter.
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }
       
       /// <summary>
       /// Type of candle argument.
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. Then you need to create your own data type in the [DataType](xref:StockSharp.Messages.DataType) class:

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// Define a unique MessageType for delta-candles.
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;
       
       /// <summary>
       /// <see cref="DeltaCandleMessage"/> data type.
       /// </summary>
       public static readonly DataType CandleDelta = 
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();
       
       /// <summary>
       /// Create a data type for delta-candles.
       /// </summary>
       /// <param name="threshold">Delta threshold value.</param>
       /// <returns>Data type.</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }
       
       /// <summary>
       /// Register the delta-candle type in the system.
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // Register new candle type in StockSharp
           Extensions.RegisterCandleMessageType(typeof(DeltaCandleMessage));
       }
   }
   ```

3. Next, you need to create a candle builder for the new type. To do this, create an implementation of [CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1):

   ```cs
   /// <summary>
   /// Candle builder for <see cref="DeltaCandleMessage"/> type.
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// Initializes a new instance of <see cref="DeltaCandleBuilder"/>.
       /// </summary>
       /// <param name="exchangeInfoProvider">Exchange information provider.</param>
       public DeltaCandleBuilder(IExchangeInfoProvider exchangeInfoProvider)
           : base(exchangeInfoProvider)
       {
       }
       
       /// <inheritdoc />
       protected override DeltaCandleMessage CreateCandle(ICandleBuilderSubscription subscription, ICandleBuilderValueTransform transform)
       {
           var time = transform.Time;
           
           return FirstInitCandle(subscription, new DeltaCandleMessage
           {
               DeltaThreshold = subscription.Message.GetArg<decimal>(),
               OpenTime = time,
               CloseTime = time,
               HighTime = time,
               LowTime = time,
               CurrentDelta = 0
           }, transform);
       }
       
       /// <inheritdoc />
       protected override bool IsCandleFinishedBeforeChange(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           // The candle closes when the absolute value of delta exceeds the threshold
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }
       
       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);
           
           // Update delta based on the trade side
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. Then you need to register the candle builder in [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider):

   ```cs
   private Connector _connector;
   ...
   // Register delta-candle type in the system
   DeltaCandleHelper.RegisterDeltaCandleType();
   
   // Register the delta-candle builder
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. Create a subscription for the `DeltaCandleMessage` type candles and request data from it:

   ```cs
   // Delta threshold value
   decimal deltaThreshold = 1000m;
   
   // Create a subscription for delta-candles
   var subscription = new Subscription(
       // Use our extension method to create a data type
       deltaThreshold.Delta(), 
       security)
   {
       MarketData =
       {
           // Specify that candles will be built from ticks
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };
   
   // Subscribe to the candle received event
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;
       
       var deltaCandle = (DeltaCandleMessage)candle;
       
       // Process delta-candle
       Console.WriteLine($"Delta-candle {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };
   
   // Subscribe to the online mode transition
   _connector.SubscriptionOnline += sub => 
   {
       if (sub == subscription)
           Console.WriteLine("Delta-candle subscription has transitioned to online mode");
   };
   
   // Start the subscription
   _connector.Subscribe(subscription);
   ```

## Using Delta-candles in Trading Strategies

Example of a simple strategy using delta-candles:

```cs
public class DeltaCandleStrategy : Strategy
{
	private readonly StrategyParam<decimal> _deltaThreshold;
	private readonly StrategyParam<decimal> _volume;
	private readonly StrategyParam<decimal> _signalDelta;

	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _deltaIndicatorElement;

	public decimal DeltaThreshold
	{
		get => _deltaThreshold.Value;
		set => _deltaThreshold.Value = value;
	}

	public decimal Volume
	{
		get => _volume.Value;
		set => _volume.Value = value;
	}

	public decimal SignalDelta
	{
		get => _signalDelta.Value;
		set => _signalDelta.Value = value;
	}

	public DeltaCandleStrategy()
	{
		// Strategy parameters
		_deltaThreshold = Param(nameof(DeltaThreshold), 1000m)
			.SetDisplay("Delta Threshold Value", "Volume delta value for candle formation", "Main Settings")
			.SetGreaterThanZero()
			.SetCanOptimize(true)
			.SetOptimize(500m, 2000m, 100m);

		_volume = Param(nameof(Volume), 1m)
			.SetDisplay("Order Volume", "Volume for trading operations", "Main Settings")
			.SetGreaterThanZero();

		_signalDelta = Param(nameof(SignalDelta), 500m)
			.SetDisplay("Minimum Delta for Signal", "Minimum delta value for signal generation", "Main Settings")
			.SetGreaterThanZero()
			.SetCanOptimize(true);

		Name = "DeltaCandleStrategy";
	}

	protected override void OnStarted(DateTimeOffset time)
	{
		base.OnStarted(time);

		// Chart initialization, if available
		_chart = GetChart();
		if (_chart != null)
		{
			var area = _chart.AddArea();
			_chartCandleElement = area.AddCandles();
			_deltaIndicatorElement = area.AddIndicator();
			_deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
			_deltaIndicatorElement.Color = System.Drawing.Color.Purple;
		}

		// Create a subscription for delta-candles
		var subscription = new Subscription(DeltaThreshold.Delta(), Security)
		{
			MarketData =
			{
				BuildMode = MarketDataBuildModes.Build,
				BuildFrom = DataType.Ticks
			}
		};

		// Create a rule for processing delta-candles
		this
			.WhenCandleReceived(subscription)
			.Do(ProcessDeltaCandle)
			.Apply(this);

		// Start the subscription
		Subscribe(subscription);
	}

	private void ProcessDeltaCandle(ICandleMessage candle)
	{
		// Draw on the chart, if available
		if (_chart != null)
		{
			var deltaCandle = (DeltaCandleMessage)candle;
			
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);
				
			_chart.Draw(data);
		}

		// Process only finished candles
		if (candle.State != CandleStates.Finished)
			return;
			
		var deltaCandle = (DeltaCandleMessage)candle;
		
		// Check if delta is sufficient for a signal
		if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
		{
			this.AddInfoLog($"Delta {deltaCandle.CurrentDelta} is less than the threshold value {SignalDelta}. No signal is generated.");
			return;
		}

		// Operation direction depends on the delta sign
		var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;
		
		this.AddInfoLog($"Delta-candle completed. Delta: {deltaCandle.CurrentDelta}. Direction: {direction}");
		
		// Use the candle's close price to determine the price
		var price = deltaCandle.ClosePrice;
		var volume = Volume;
		
		// If we already have a position in the opposite direction, 
		// increase the volume to close the existing position
		if ((Position < 0 && direction == Sides.Buy) || 
			(Position > 0 && direction == Sides.Sell))
		{
			volume = Math.Max(volume, Math.Abs(Position) + volume);
		}
		
		// Register an order
		RegisterOrder(this.CreateOrder(direction, price, volume));
	}
}
```

## Important Points When Creating Custom Candle Types

1. **Uniqueness of MessageTypes** — make sure that the `MessageTypes` identifier you choose does not conflict with existing types in StockSharp. It is recommended to use values greater than 10000 for custom types.

2. **Registration of the Candle Type** — registration through `Extensions.RegisterCandleMessageType` is necessary for proper integration with StockSharp graphical controls and data storage. Without registration, the candle type will only work in code but will not be available in the user interface.

3. **Candle Parameter** — implement the `ArgType` property that returns the type of the candle argument. This is used for correct display of parameters in the graphical interface.

Thus, we have created a completely custom candle type that properly integrates with the entire StockSharp ecosystem (including the user interface and data storage) and can be used to build trading strategies based on volume delta analysis.