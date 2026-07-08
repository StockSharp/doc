# 自定义K线类型

[S#](../../api.md) 支持通过自定义K线类型扩展K线构建功能。当需要使用 [S#](../../api.md) 尚未内置支持的K线时，此功能非常有用。下面以 Delta K线（根据买入量与卖出量之差形成的K线）为例，介绍创建自定义K线类型的完整过程。

## 实现 Delta K线

1. 首先创建自定义K线消息类型。该类型必须继承 [CandleMessage](xref:StockSharp.Messages.CandleMessage)：

   ```cs
   /// <summary>
   /// 基于买入和卖出成交量差值形成的 K线。
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
       // 从辅助方法获取消息类型标识符
       // 以便在 RegisterCandleType 中使用相同值
       
       /// <summary>
       /// 初始化 <see cref="DeltaCandleMessage"/> 的新实例。
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }
       
       /// <summary>
       /// K线形成的 delta 阈值。
       /// </summary>
       public decimal DeltaThreshold { get; set; }
       
       /// <summary>
       /// 当前 delta 值。
       /// </summary>
       public decimal CurrentDelta { get; set; }
       
       /// <summary>
       /// 创建 <see cref="DeltaCandleMessage"/> 的副本。
       /// </summary>
       /// <returns>副本。</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }
       
       /// <summary>
       /// K线参数。
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }
       
       /// <summary>
       /// K线参数类型。
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. 然后在 [DataType](xref:StockSharp.Messages.DataType) 中创建自定义数据类型：

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// 为 delta-K线定义唯一的 MessageType。
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;
       
       /// <summary>
       /// <see cref="DeltaCandleMessage"/> 数据类型。
       /// </summary>
       public static readonly DataType CandleDelta = 
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();
       
       /// <summary>
       /// 创建 delta-K线的数据类型。
       /// </summary>
       /// <param name="threshold">delta 阈值。</param>
       /// <returns>数据类型。</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }
       
       /// <summary>
       /// 在系统中注册 delta-K线类型。
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // 在 StockSharp 中注册新的 K线类型
           Extensions.RegisterCandleType<decimal>(
               typeof(DeltaCandleMessage),      // Candle message type
               DeltaCandleType,                // Message type
               "delta",                        // File name for storage
               str => str.To<decimal>(),       // Converter from string to parameter
               arg => arg.ToString(),          // Converter from parameter to string
               a => a > 0,                     // Parameter validator
               false                           // Whether such candles can be obtained from the source (not only built)
           );
       }
   }
   ```

3. 接下来为新类型创建K线构建器，即实现 [CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1)：

   ```cs
   /// <summary>
   /// <see cref="DeltaCandleMessage"/> 类型的 K线构建器。
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// 初始化 <see cref="DeltaCandleBuilder"/> 的新实例。
       /// </summary>
       /// <param name="exchangeInfoProvider">交易所信息提供者。</param>
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
           // 当 delta 的绝对值超过阈值时 K线 收盘
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }
       
       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);
           
           // 根据成交方向更新 delta
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. 在 [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider) 中注册K线构建器：

   ```cs
   private Connector _connector;
   ...
   // 在系统中注册 delta-K线类型
   DeltaCandleHelper.RegisterDeltaCandleType();
   
   // 注册 delta-K线构建器
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. 为 `DeltaCandleMessage` 类型的K线创建订阅并请求数据：

   ```cs
   // delta 阈值
   decimal deltaThreshold = 1000m;
   
   // 创建 delta-K线订阅
   var subscription = new Subscription(
       // 使用我们的扩展方法创建数据类型
       deltaThreshold.Delta(), 
       security)
   {
       MarketData =
       {
           // 指定 K线将从 tick 构建
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };
   
   // 订阅 K线接收事件
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;
       
       var deltaCandle = (DeltaCandleMessage)candle;
       
       // 处理 delta-K线
       Console.WriteLine($"Delta-candle {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };
   
   // 订阅切换到在线模式的事件
   _connector.SubscriptionOnline += sub => 
   {
       if (sub == subscription)
           Console.WriteLine("Delta-candle subscription has transitioned to online mode");
   };
   
   // 启动订阅
   _connector.Subscribe(subscription);
   ```

## 在交易策略中使用 Delta K线

以下是使用 Delta K线的简单策略示例：

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
		// 策略参数
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

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// 如果可用，则初始化图表
		_chart = GetChart();
		if (_chart != null)
		{
			var area = _chart.AddArea();
			_chartCandleElement = area.AddCandles();
			_deltaIndicatorElement = area.AddIndicator();
			_deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
			_deltaIndicatorElement.Color = System.Drawing.Color.Purple;
		}

		// 创建 delta-K线订阅
		var subscription = new Subscription(DeltaThreshold.Delta(), Security)
		{
			MarketData =
			{
				BuildMode = MarketDataBuildModes.Build,
				BuildFrom = DataType.Ticks
			}
		};

		// 创建处理 delta-K线的规则
		this
			.WhenCandleReceived(subscription)
			.Do(ProcessDeltaCandle)
			.Apply(this);

		// 启动订阅
		Subscribe(subscription);
	}

	private void ProcessDeltaCandle(ICandleMessage candle)
	{
		// 如果图表可用，则绘制到图表上
		if (_chart != null)
		{
			var deltaCandle = (DeltaCandleMessage)candle;
			
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);
				
			_chart.Draw(data);
		}

		// 只处理已完成的 K线
		if (candle.State != CandleStates.Finished)
			return;
			
		var deltaCandle = (DeltaCandleMessage)candle;
		
		// 检查 delta 是否足以形成信号
		if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
		{
			this.AddInfoLog($"Delta {deltaCandle.CurrentDelta} is less than the threshold value {SignalDelta}. No signal is generated.");
			return;
		}

		// 操作方向取决于 delta 符号
		var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;
		
		this.AddInfoLog($"Delta-candle completed. Delta: {deltaCandle.CurrentDelta}. Direction: {direction}");
		
		// 使用 K线收盘价确定价格
		var price = deltaCandle.ClosePrice;
		var volume = Volume;
		
		// 如果 已经有反向仓位，
		// 增加数量以平掉现有仓位
		if ((Position < 0 && direction == Sides.Buy) || 
			(Position > 0 && direction == Sides.Sell))
		{
			volume = Math.Max(volume, Math.Abs(Position) + volume);
		}
		
		// 注册订单
		RegisterOrder(this.CreateOrder(direction, price, volume));
	}
}
```

## 创建自定义K线类型时的注意事项

1. **MessageTypes 的唯一性** — 确保所选 `MessageTypes` 标识符不与 StockSharp 中的现有类型冲突。建议自定义类型使用大于 10000 的值。

2. **注册K线类型** — 必须通过 `Extensions.RegisterCandleType` 注册，才能正确集成 StockSharp 图形控件和数据存储。未注册的K线类型只能在代码中使用，不会出现在用户界面中。

3. **K线参数** — 实现返回K线参数类型的 `ArgType` 属性，以便在图形界面中正确显示参数。

4. **文件系统** — 使用 StockSharp 数据存储时，`RegisterCandleType` 方法的 `fileName` 参数用于在文件系统中保存K线。

5. **参数验证** — StockSharp 会在创建订阅前调用参数验证方法，检查参数值是否有效。

至此，自定义K线类型已经创建完成。它能够与整个 StockSharp 生态系统（包括用户界面和数据存储）集成，并可用于构建基于成交量 Delta 分析的交易策略。
