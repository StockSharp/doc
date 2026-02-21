# Indicators in Strategy

In StockSharp, the [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class provides a special mechanism for working with indicators, which allows you to control their formation state and determine when the strategy is ready to work.

## Indicators Property

The [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) property is a collection of indicators used in the strategy. This collection is designed to automatically track the state of indicator formation (warm-up).

```cs
// Accessing the indicators collection
INotifyList<IIndicator> indicators = strategy.Indicators;
```

## IsFormed Property

By default, the implementation of the [Strategy.IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) property checks if all indicators in the [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) collection are formed:

```cs
// Standard implementation in the Strategy class
public virtual bool IsFormed => _indicators.AllFormed;
```

A strategy is considered "warmed up" and ready to work when all indicators in the collection are formed (their [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) property returns `true`).

## Adding Indicators to the Collection

To correctly determine when the strategy is ready, you need to add the indicators you use to the [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) collection:

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

## Which Indicators to Add

You should only add **independent indicators** to the [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) collection. This is an important rule that will help avoid unnecessary waiting and correctly determine when the strategy is ready.

### Rules for Adding Indicators:

1. **Independent Indicators** - add indicators that directly process market data (candles, ticks, etc.):

   ```cs
   // Independent indicators
   var sma = new SimpleMovingAverage { Length = 20 };
   var rsi = new RelativeStrengthIndex { Length = 14 };
   
   Indicators.Add(sma);
   Indicators.Add(rsi);
   ```

2. **Indicator Chains** - when using an indicator chain (where the output of one is the input for another), add only the **first indicator in the chain** to the collection:

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

3. **Combined Indicators** - for indicators that use multiple independent indicators (e.g., MACD), add all of them:

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

## Usage Examples

### Basic Example with Two Moving Averages

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

### Example Using IsFormedAndOnline

To check if the strategy is ready for trading, the [IsFormedAndOnlineAndAllowTrading](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) method is often used, which combines checking indicator formation, online status, and trading permission:

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

## Optimizing Indicator Usage

In more complex strategies, it's important to properly organize work with indicators:

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

## Advanced Features

You can override the [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) property in your strategy if the standard behavior is not sufficient:

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

## See also

- [List of Indicators](../indicators/list_of_indicators.md)
- [Custom Indicator](../indicators/custom_indicator.md)
- [Strategy Compatibility with Platforms](compatibility.md)
