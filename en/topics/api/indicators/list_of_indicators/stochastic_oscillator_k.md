# Stochastic Oscillator %K

**Stochastic Oscillator %K** is a component of the stochastic oscillator that shows the current close price position relative to the price range over the selected period. The indicator was developed by George Lane in the late 1950s.

To use the indicator, use the [StochasticK](xref:StockSharp.Algo.Indicators.StochasticK) class.

## Description

Stochastic Oscillator %K is based on the observation that during uptrends, closing prices usually concentrate closer to the upper boundary of the price range, while during downtrends they tend to concentrate closer to the lower boundary.

%K is the "fast" line of the stochastic oscillator and is the main component used to calculate the %D line, which is a moving average of %K.

The oscillator ranges from 0 to 100:

- Values above 80 usually indicate an overbought market.
- Values below 20 indicate an oversold market.
- Crossovers of the %K and %D lines can be used as entry or exit signals.

## Parameters

- **Length** - period used to calculate the price range, meaning the highs and lows. The common default is 14.

## Calculation

Formula for calculating %K:

```
%K = 100 * ((Close - Low(Length)) / (High(Length) - Low(Length)))
```

where:

- Close - current close price.
- Low(Length) - minimum price over the Length period.
- High(Length) - maximum price over the Length period.

In the full stochastic oscillator, the %D line is calculated as a simple moving average of %K over the specified period, usually 3:

```
%D = SMA(%K, 3)
```

![IndicatorStochasticK](../../../../images/indicatorstochastick.png)

## See also

[Stochastic Oscillator](stochastic_oscillator.md)
