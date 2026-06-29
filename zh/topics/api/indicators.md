# 指标

[S\#](../api.md) 提供了超过 140 种标准技术分析指标。这使您可以使用现成的指标，而不是从头创建。您还可以根据现有指标创建自己的指标，如 [自定义指标](indicators/custom_indicator.md) 部分所示。所有用于处理指标的基类以及指标本身都位于 [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators) 命名空间中。

## 将指标整合到交易算法中

1. 首先，您需要创建一个指标。指标的创建方式就像创建常规的 .NET 对象一样：

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   var shortSma = new SimpleMovingAverage { Length = 30 };
   
   // It's recommended to add indicators to the strategy collection
   Indicators.Add(longSma);
   Indicators.Add(shortSma);
   ```

2. 接下来，您需要处理指标的市场数据。最有效的方法是使用 [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) 方法返回的结果：

   ```cs
   private void ProcessCandle(ICandleMessage candle)
   {
       // Process the candle with indicators and immediately save the results
       var longValue = longSma.Process(candle);
       var shortValue = shortSma.Process(candle);
       
       // Use the results for trading decisions
       if (shortValue.GetValue<decimal>() > longValue.GetValue<decimal>())
       {
           // Buy signal
           BuyAtMarket();
       }
   }
   ```

指标接受 [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) 作为输入。一些指标使用简单数字，例如 [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)。其他指标则需要完整的蜡烛数据，例如 [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice)。因此，输入值需要被转换为 [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) 或 [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue)。指标的结果值遵循与输入值相同的规则。

3. 指标的结果值和输入值都具有 [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) 属性，该属性表示该值是最终值，指标在此时不会变化。例如，[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) 指标是基于蜡烛的收盘价形成的，但在当前时刻，最终收盘价未知且在变化。在这种情况下，[IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) 的结果值将为 false。如果你将一个已完成的蜡烛传入指标，[IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) 的输入值和结果值都将为 true。

4. **推荐方法**：直接使用从调用 [Process](xref:StockSharp.Algo.Indicators.IIndicator.Process(StockSharp.Algo.Indicators.IIndicatorValue)) 方法获得的值，而不是随后调用 [GetCurrentValue](xref:StockSharp.Algo.Indicators.IndicatorHelper.GetCurrentValue(StockSharp.Algo.Indicators.IIndicator))：

   ```cs
   // Example of a strategy with two moving averages
   private void ProcessCandle(ICandleMessage candle)
   {
       // Process the candle with indicators and immediately save the results
       var longValue = _longSma.Process(candle);
       var shortValue = _shortSma.Process(candle);
       
       // Draw on the chart
       DrawCandlesAndIndicators(candle, longValue, shortValue);
       
       if (!IsFormedAndOnlineAndAllowTrading()) 
           return;
           
       // Use the obtained values for comparison
       var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
       var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);
       
       // Check if a crossover occurred
       if (isShortLessCurrent == isShortLessPrev) 
           return;
       
       var volume = Volume + Math.Abs(Position);
       
       // Trading actions based on the signal
       if (isShortLessCurrent)
           SellMarket(volume);
       else
           BuyMarket(volume);
   }
   ```

这种方法具有以下优点：
   - 它与流数据处理模型一致（接收 → 处理 → 使用结果）
   - 它更高效，因为它避免了反复访问累积值的容器
   - 它消除了调用 Process 与随后的 GetCurrentValue 调用之间的潜在同步问题

5. 不推荐的方法（效率较低）：

   ```cs
   // Suboptimal approach
   foreach (var candle in candles)
   {
       // Process the candle but ignore the returned value
       _longSma.Process(candle);
       _shortSma.Process(candle);
   }
   
   // Later try to get values via GetCurrentValue()
   var isShortLessThenLong = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
   ```

使用这种方法，会额外访问历史指标值的容器，这会引入延迟并扰乱数据处理的流式模型。

6. 所有指标都有 [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) 属性，该属性表示指标是否可以使用。例如，[SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) 指标有一个周期，在指标处理的蜡烛数量达到指标的周期之前，该指标将被视为不可用。而 [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) 属性将为假。

## 完整移动平均策略示例

下面是一个策略示例，它正确使用指标、处理K线并利用Process方法的结果：

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;

	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;
	private IChartCandleElement _chartCandleElement;
	private IChartTradeElement _tradesElem;
	private IChart _chart;

	public SmaStrategy()
	{
		base.Name = "SMA strategy";

		// Initialize strategy parameters
		_longSmaLength = Param(nameof(LongSmaLength), 80);
		_shortSmaLength = Param(nameof(ShortSmaLength), 30);
		_series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)));
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Create indicators
		_shortSma = new SimpleMovingAverage { Length = _shortSmaLength.Value };
		_longSma = new SimpleMovingAverage { Length = _longSmaLength.Value };

		// Add indicators to the strategy collection
		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);

		// Initialize chart
		_chart = GetChart();
		if (_chart != null)
		{
			InitChart();
		}
		
		// Subscribe to candles
		var subscription = new Subscription(_series.Value, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Connector.Subscribe(subscription);
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// Process the candle with indicators and save the results
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Draw on the chart
		DrawCandlesAndIndicators(candle, longValue, shortValue);
		
		// Check conditions for trading
		if (!IsFormedAndOnlineAndAllowTrading()) 
			return;

		// Compare current and previous indicator values
		var isShortLessCurrent = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		// Check for crossover
		if (isShortLessCurrent == isShortLessPrev) 
			return;

		var volume = Volume + Math.Abs(Position);

		// Trading actions based on the signal
		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}

	private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
	{
		if (_chart == null) return;
		var data = _chart.CreateData();
		data.Group(candle.OpenTime)
			.Add(_chartCandleElement, candle)
			.Add(_longSmaIndicatorElement, longSma)
			.Add(_shortSmaIndicatorElement, shortSma);
		_chart.Draw(data);
	}

	// Other chart initialization methods omitted for brevity
}
```

本示例演示了在 StockSharp 流式模型中使用指标的正确方法。
