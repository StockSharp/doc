# Example of a Strategy in C#

Creating a strategy from source code will be illustrated using the example of an SMA strategy - a similar SMA strategy example, assembled from cubes in the section [Creating an Algorithm from Cubes](Designer_Algorithm_creation_of_elements.md).

This section will not describe the constructs of the C# language (as well as [Strategy](Strategy.md), on the basis of which strategies are created), but features for the code to work in **Designer** will be mentioned.

> [!TIP]
> Strategies created in **Designer** are compatible with strategies created in [API](StockSharpAbout.md) due to the use of a common base class [Strategy](Strategy.md). This makes running such strategies outside of **Designer** significantly easier than [diagrams](Designer_run_strategy_on_server.md).

1. Strategy parameters are created through a special approach:

```cs
_candleTypeParam = this.Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);


private readonly StrategyParam<DataType> _candleTypeParam;

public DataType CandleType
{
	get => _candleTypeParam.Value;
	set => _candleTypeParam.Value = value;
}

private readonly StrategyParam<int> _long;

public int Long
{
	get => _long.Value;
	set => _long.Value = value;
}

private readonly StrategyParam<int> _short;

public int Short
{
	get => _short.Value;
	set => _short.Value = value;
}
```

Using the [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) class automatically employs the approach to save and restore settings.

2. When creating indicators, it is necessary to add them to the internal strategy list ([Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators)):

```cs
_longSma = new SimpleMovingAverage { Length = Long };
_shortSma = new SimpleMovingAverage { Length = Short };

// !!! DO NOT FORGET add it in case use IsFormed property (see code below)
Indicators.Add(_longSma);
Indicators.Add(_shortSma);
```

This is done so that the strategy can track all the indicators necessary for its logic when they become [formed](Indicators.md).

3. When working with the chart, it is necessary to consider that in the case of launching the strategy [outside Designer](Designer_run_strategy_on_server.md), the chart object may be absent.

```cs
_chart = this.GetChart();

// chart can be NULL in case hosting strategy in custom app like Runner or Shell
if (_chart != null)
{
	var area = _chart.AddArea();

	_chartCandlesElem = area.AddCandles();
	_chartTradesElem = area.AddTrades();
	_chartShortElem = area.AddIndicator(_shortSma);
	_chartLongElem = area.AddIndicator(_longSma);

	// you can apply custom color palette here
}
```

And further in the code for drawing data on the chart, make special checks like:

```cs
if (_chart == null)
	return;

var data = _chart.CreateData();
```

4. To ensure that the strategy is fully ready to work, its indicators are formed with historical data and all subscriptions have transitioned to [Online](API_ConnectorsSubscriptions.md), a special method can be used:

```cs
// some of indicators added in OnStarted not yet fully formed
// or user turned off allow trading
if (this.IsFormedAndOnlineAndAllowTrading())
{
	// TODO
}
```