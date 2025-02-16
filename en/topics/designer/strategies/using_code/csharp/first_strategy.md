# C# Strategy Example

Creating a strategy from source code will be demonstrated using the SMA strategy example - similar to the SMA strategy example assembled from cubes in the [Creating Algorithm from Cubes](../../using_visual_designer/first_strategy.md) section.

This section will not describe C# language constructs (or [Strategy](../../../../api/strategies.md), which is used as the base for creating strategies), but will mention specific features for working with code in the **Designer**.

> [!TIP]
> Strategies created in the **Designer** are compatible with strategies created in the [API](../../../../api.md) due to using the common base class [Strategy](../../../../api/strategies.md). This makes running such strategies outside the **Designer** significantly easier than running [schemes](../../../live_execution/running_strategies_outside_of_designer.md).

1. Strategy parameters are created using a special approach:

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

When using the [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) class, the settings save and restore approach is automatically used.

2. When creating indicators and subscribing to market data, you need to bind them so that incoming data from the subscription can update indicator values:

```cs
// ---------- create indicators -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- bind candles set and indicators ----

var subscription = SubscribeCandles(CandleType);

subscription
    // bind indicators to the candles
    .Bind(longSma, shortSma, OnProcess)
    // start processing
    .Start();
```

3. When working with charts, it's important to consider that when running the strategy [outside the Designer](../../../live_execution/running_strategies_outside_of_designer.md), the chart object might be absent.

```cs
var area = CreateChartArea();

// area can be null in case of no GUI (strategy hosted in Runner or in own console app)
if (area != null)
{
    DrawCandles(area, subscription);

    DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
    DrawIndicator(area, longSma);

    DrawOwnTrades(area);
}
```

4. Start position protection through [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) if required by the strategy logic:

```cs
// start protection by take profit and-or stop loss
StartProtection(TakeValue, StopValue);
```

5. The strategy logic itself, implemented in the OnProcess method. The method is called by the subscription created in step 1:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
    LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

    // in case we subscribed on non finished only candles
    if (candle.State != CandleStates.Finished)
        return;

    // calc new values for short and long
    var isShortLessThenLong = shortValue < longValue;

    if (_isShortLessThenLong == null)
    {
        _isShortLessThenLong = isShortLessThenLong;
    }
    else if (_isShortLessThenLong != isShortLessThenLong)
    {
        // crossing happened

        // if short less than long, the sale, otherwise buy
        var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

        // calc size for open position or revert
        var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

        var priceStep = GetSecurity().PriceStep ?? 1;

        // calc order price as a close price + offset
        var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

        if (direction == Sides.Buy)
            BuyLimit(price, volume);
        else
            SellLimit(price, volume);

        // store current values for short and long
        _isShortLessThenLong = isShortLessThenLong;
    }
}
```