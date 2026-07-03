# 战略中的指标

在 StockSharp 中，[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类提供了一种用于处理指标的特殊机制，这使您能够控制它们的形成状态，并确定策略何时可以开始工作。

## 指标属性

[Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 属性是一组用于策略的指标集合。该集合旨在自动跟踪指标形成的状态（预热）。

```cs
// Accessing the indicators collection
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## IsFormed 属性

默认情况下，[Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) 属性的实现会检查 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中的所有指标是否已形成：

```cs
// Standard implementation in the Strategy class
public virtual bool IsFormed => _indicators.AllFormed;
```

当集合中的所有指标都已形成（它们的 [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) 属性返回 `true`）时，策略被认为是“预热”并准备工作。

## 将指标添加到集合中

要正确判断策略何时准备好，您需要将使用的指标添加到 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Creating indicators
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// Adding indicators to the collection
	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	// ...
}
```

## 添加哪些指标

您应该只向 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中添加 **独立指标**。这是一个重要规则，有助于避免不必要的等待，并正确判断策略何时准备就绪。

### 添加指标的规则：

1. **独立指标** - 添加直接处理市场数据（K线、逐笔数据等）的指标：

   ```cs
   // Independent indicators
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **指标链** - 当使用指标链（一个指标的输出作为另一个指标的输入）时，仅将**链中的第一个指标**添加到集合中：

   ```cs
   // Indicator chain
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands 
   { 
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };
   
   // Add only the first indicator in the chain
   Indicators.Add(sma);
   // DO NOT add indicators dependent on other indicators
   // Indicators.Add(stdev); - incorrect
   // Indicators.Add(bollingerBands); - incorrect
   ```

3. **组合指标** - 对于使用多个独立指标的指标（e.g., MACD），将它们全部相加：

   ```cs
   var fastEma = new ExponentialMovingAverage { Length = 12 };
   var slowEma = new ExponentialMovingAverage { Length = 26 };
   var signalEma = new ExponentialMovingAverage { Length = 9 };
   var macd = new MovingAverageConvergenceDivergence
   {
       FastEma = fastEma,
       SlowEma = slowEma,
       SignalEma = signalEma
   };
   
   // Add base indicators
   Indicators.Add(fastEma);
   Indicators.Add(slowEma);
   ```

## 使用示例

### 带有两个移动平均线的基本示例

```cs
public class SmaStrategy : Strategy
{
	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	
	// ...
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
		
		// Add indicators to the collection to track their state
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);
		
		// ...
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Process indicators
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// Check if the strategy is ready before executing trading logic
		if (!IsFormed)
			return;
			
		// Trading logic
		// ...
	}
}
```

### 使用 IsFormedAndOnline 的示例

为了检查策略是否准备好进行交易，通常使用 [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) 方法，该方法结合了检查指标形成、在线状态和交易许可：

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Process indicators
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);
	
	// Comprehensive check of strategy readiness
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
		
	// Trading logic
	// ...
}
```

## 优化指标使用

在更复杂的策略中，正确地使用指标来组织工作非常重要：

```cs
public class ComplexStrategy : Strategy
{
	private SimpleMovingAverage _sma;
	private RelativeStrengthIndex _rsi;
	private BollingerBands _bollinger;
	private StandardDeviation _stdev;
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// Create indicators
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };
		
		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands 
		{ 
			SmaIndicator = _sma,
			DeviationIndicator = _stdev 
		};
		
		// Add only independent indicators
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// Do not add _stdev and _bollinger as they depend on _sma
		
		// ...
	}
	
	// ...
}
```

## 高级功能

如果标准行为不够用，您可以在策略中重写 [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) 属性：

```cs
public override bool IsFormed
{
	get
	{
		// Standard indicator check
		if (!base.IsFormed)
			return false;
			
		// Additional strategy readiness conditions
		return _customCondition && _additionalCheck;
	}
}
```

## 另请参阅

- [指标列表](../indicators/list_of_indicators.md)
- [自定义指标](../indicators/custom_indicator.md)
- [策略与平台的兼容性](compatibility.md)
