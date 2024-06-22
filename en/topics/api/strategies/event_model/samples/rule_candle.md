# Candle Rule Strategy

## Overview

`SimpleCandleRulesStrategy` is a strategy that demonstrates the use of rules for candles in StockSharp. It tracks candle volumes and logs information when certain conditions are met.

## Main Components

```cs
// Main components
public class SimpleCandleRulesStrategy : Strategy
{
    private Subscription _subscription;
}
```

## OnStarted Method

Called when the strategy starts:

- Initializes subscription to 5-minute candles
- Sets up rules for processing candles

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    _subscription = new(Security.TimeFrame(TimeSpan.FromMinutes(5)))
    {
        // ready-to-use candles much faster than compression on fly mode
        // turn off compression to boost optimizer (!!! make sure you have candles)

        //MarketData =
        //{
        //    BuildMode = MarketDataBuildModes.Build,
        //    BuildFrom = DataType.Ticks,
        //}
    };
    Subscribe(_subscription);

    var i = 0;

    this.WhenCandlesStarted(_subscription)
        .Do((candle) =>
        {
            i++;

            this
                .WhenTotalVolumeMore(candle, new Unit(100000m))
                .Do((candle1) =>
                {
                    this.AddInfoLog($"The rule WhenPartiallyFinished and WhenTotalVolumeMore candle={candle1}");
                    this.AddInfoLog($"The rule WhenPartiallyFinished and WhenTotalVolumeMore i={i}");
                }).Apply(this);

        }).Apply(this);

    base.OnStarted(time);
}
```

## Working Logic

- The strategy subscribes to 5-minute candles
- A rule is set up at the start of each candle formation
- The rule triggers when the total candle volume exceeds 100,000
- When the rule triggers, information about the candle and counter is added to the log

## Features

- Demonstrates the use of `WhenCandlesStarted` and `WhenTotalVolumeMore` rules
- Uses the candle subscription mechanism
- Shows an example of logging information in the strategy
- Contains commented-out code for setting up candle building from ticks