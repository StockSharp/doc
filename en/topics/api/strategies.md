# Strategies

## Introduction

StockSharp provides a powerful toolkit for creating trading strategies. In this guide, we will explore the process of creating a strategy using the example of an SMA (Simple Moving Average) strategy.

## Basics of Strategy Creation

### Inheriting from the Base Class

To create a strategy, you need to create a class that inherits from [Strategy](xref:StockSharp.Algo.Strategies.Strategy).

```cs
public class SmaStrategy : Strategy
{
    // Declaring a strategy class inheriting from the base Strategy class
    // This allows using all basic functions of StockSharp strategies
}
```

### Strategy Parameters

Strategy parameters are defined using [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1). This allows for easy configuration without changing the code.

```cs
// Declaring strategy parameters
private readonly StrategyParam<DataType> _candleTypeParam;
private readonly StrategyParam<int> _long;
private readonly StrategyParam<int> _short;

public DataType CandleType
{
    get => _candleTypeParam.Value;
    set => _candleTypeParam.Value = value;
}

public int Long
{
    get => _long.Value;
    set => _long.Value = value;
}

public int Short
{
    get => _short.Value;
    set => _short.Value = value;
}

// These parameters allow easy configuration of the strategy without changing the code
// CandleType defines the type of candles used
// Long and Short set the lengths for the long and short SMAs
```

## Strategy Initialization

### OnStarted Method

The [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method is called when the strategy starts and is used for initialization.

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);

    this.AddInfoLog(nameof(OnStarted));

    // The OnStarted method is called when the strategy starts
    // The main initialization occurs here
}
```

### Creating Indicators

This method creates and configures the indicators used.

```cs
// Creating indicators
_longSma = new SimpleMovingAverage { Length = Long };
_shortSma = new SimpleMovingAverage { Length = Short };

Indicators.Add(_longSma);
Indicators.Add(_shortSma);

// Creating two SMA indicators with different periods
// It is important to add them to the Indicators collection for proper operation
```

### Subscribing to Data

Here, we subscribe to the necessary market data.

```cs
// Subscribing to market data
var subscription = new Subscription(CandleType, Security)
{
    MarketData =
    {
        IsFinishedOnly = true,
    }
};

subscription
    .WhenCandleReceived(this)
    .Do(ProcessCandle)
    .Apply(this);

Subscribe(subscription);

// Creating a subscription for candles of the selected type
// IsFinishedOnly = true means only completed candles are processed
// WhenCandleReceived subscribes to receiving new candles
// ProcessCandle is the method that will be called for each new candle
```

A [rule](strategies/event_model.md) is created for the subscription, activating each time a candle arrives.

## Processing Market Data

### ProcessCandle Method

This method is called for each new candle and contains the main logic of the strategy.

```cs
private void ProcessCandle(ICandleMessage candle)
{
    // The ProcessCandle method is called for each new candle
    // The main logic of the strategy is implemented here
}
```

### Trading Logic

This method implements the core trading logic, including indicator analysis and decision-making for entering or exiting a position.

```cs
// Trading logic
if (this.IsFormedAndOnlineAndAllowTrading())
{
    if (candle.State == CandleStates.Finished)
    {
        var isShortLessThenLong = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();

        if (_isShortLessThenLong == null)
        {
            _isShortLessThenLong = isShortLessThenLong;
        }
        else if (_isShortLessThenLong != isShortLessThenLong)
        {
            // Implementing the main trading logic here
            // Checking the crossover of the short and long SMAs
            // Based on this, a decision is made to buy or sell
        }
    }
}

// This code checks if the indicators are formed and trading is allowed
// Then it analyzes the crossover of the short and long SMAs
// A buy or sell signal is generated on crossover
```

## Visualization

StockSharp allows easy visualization of the strategy's performance using charts.

```cs
_chart = this.GetChart();

if (_chart != null)
{
    var area = _chart.AddArea();

    _chartCandlesElem = area.AddCandles();
    _chartTradesElem = area.AddTrades();
    _chartShortElem = area.AddIndicator(_shortSma);
    _chartLongElem = area.AddIndicator(_longSma);
}

// Code for creating and configuring a chart
// Adding elements to display candles, trades, and indicators
```

## Managing Positions and Orders

### Creating and Registering Orders

To create orders, use the [CreateOrder](xref:StockSharp.Algo.Strategies.StrategyHelper.CreateOrder(StockSharp.Algo.Strategies.Strategy,StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) helper method.

```cs
var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
var price = candle.ClosePrice + ((direction == Sides.Buy ? priceStep : -priceStep) ?? 1);

RegisterOrder(this.CreateOrder(direction, price, volume));

// Code for creating and registering an order
// Determining the trade direction (buy or sell)
// Calculating the order volume and price
// Creating and registering a new order
```

Orders are registered using the [RegisterOrder](xref:StockSharp.Algo.Strategies.Strategy.RegisterOrder(StockSharp.Messages.OrderRegisterMessage)) method.

## Handling Own Trades

The strategy can track its own trades for performance analysis.

```cs
this
    .WhenNewMyTrade()
    .Do(_myTrades.Add)
    .Apply(this);

// Code for handling own trades
// When a new trade appears, it is added to the _myTrades list
```

## Logging

Logging is essential for debugging and monitoring the strategy's performance.

```cs
this.AddInfoLog(nameof(OnStarted));
this.AddInfoLog(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

// Examples of logging
// Important events such as strategy start and receiving a new candle are logged
```