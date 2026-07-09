# 战略中的指标

在 StockSharp 中，[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类提供了一种用于处理指标的特殊机制，这使您能够控制它们的形成状态，并确定策略何时可以开始工作。

## 指标属性

[Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 属性是一组用于策略的指标集合。该集合旨在自动跟踪指标形成的状态（预热）。

```cs
// 访问指标集合
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## IsFormed 属性

默认情况下，[Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) 属性的实现会检查 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中的所有指标是否已形成：

```cs
// Strategy 类中的标准实现
public virtual bool IsFormed => _indicators.AllFormed;
```

当集合中的所有指标都已形成（它们的 [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) 属性返回 `true`）时，策略被认为是“预热”并准备工作。

## 将指标添加到集合中

要正确判断策略何时准备好，您需要将使用的指标添加到 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 创建指标
	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };
	
	// 将指标添加到集合
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
   // 独立指标
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **指标链** - 当使用指标链（一个指标的输出作为另一个指标的输入）时，仅将**链中的第一个指标**添加到集合中：

   ```cs
   // 指标链
   var sma = new SimpleMovingAverage { Length = 20 };
   var stdev = new StandardDeviation { Length = 20 };
   var bollingerBands = new BollingerBands 
   { 
       SmaIndicator = sma,
       DeviationIndicator = stdev
   };
   
   // 只添加指标链中的第一个指标
   Indicators.Add(sma);
   // 不要添加依赖其他指标的指标
   // Indicators.Add(stdev); - 错误
   // Indicators.Add(bollingerBands); - 错误
   ```

3. **组合指标** - 对于使用多个独立指标的指标（例如， MACD），将它们全部相加：

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
   
   // 添加基础指标
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
		
		// 将指标添加到集合以跟踪其状态
		Indicators.Add(_longSma);
		Indicators.Add(_shortSma);
		
		// ...
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// 处理指标
		var longValue = _longSma.Process(candle);
		var shortValue = _shortSma.Process(candle);
		
		// 执行交易逻辑前检查策略是否就绪
		if (!IsFormed)
			return;
			
		// 交易逻辑
		// ...
	}
}
```

### 使用 IsFormedAndOnline 的示例

为了检查策略是否准备好进行交易，通常使用 [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) 方法，该方法结合了检查指标形成、在线状态和交易许可：

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// 处理指标
	var longValue = _longSma.Process(candle);
	var shortValue = _shortSma.Process(candle);
	
	// 全面检查策略是否就绪
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
		
	// 交易逻辑
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
		
		// 创建指标
		_sma = new SimpleMovingAverage { Length = 20 };
		_rsi = new RelativeStrengthIndex { Length = 14 };
		
		_stdev = new StandardDeviation { Length = 20 };
		_bollinger = new BollingerBands 
		{ 
			SmaIndicator = _sma,
			DeviationIndicator = _stdev 
		};
		
		// 只添加独立指标
		Indicators.Add(_sma);
		Indicators.Add(_rsi);
		// 不要添加 _stdev 和 _bollinger，因为它们依赖 _sma
		
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
		// 标准指标检查
		if (!base.IsFormed)
			return false;
			
		// 额外的策略就绪条件
		return _customCondition && _additionalCheck;
	}
}
```

## 另请参阅

- [指标列表](../indicators/list_of_indicators.md)
- [自定义指标](../indicators/custom_indicator.md)
- [策略与平台的兼容性](compatibility.md)
