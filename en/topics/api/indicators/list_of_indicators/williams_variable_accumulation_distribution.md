# WVAD

**Williams Variable Accumulation Distribution (WVAD)** is a cumulative volume indicator developed by Larry Williams. It evaluates buying and selling pressure by analyzing the relationship between the open price, close price, high, low, and trading volume.

To use the indicator, use the [WilliamsVariableAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsVariableAccumulationDistribution) class.

## Description

The WVAD indicator measures how much buyers or sellers control price movement within each bar, and weights this value by volume. If the close price is higher than the open price, it indicates buyer dominance, and vice versa. The High-Low range is used as a normalizing factor.

Main applications of the indicator:
- Confirming the current trend
- Identifying divergences between the indicator and price
- Determining buying or selling pressure
- Assessing the strength of price movement taking volume into account

## Calculation

The WVAD indicator is calculated using the following formula:

```
WVAD = WVAD(previous) + ((Close - Open) / (High - Low)) * Volume
```

where:
- Close - closing price of the current period
- Open - opening price of the current period
- High - highest price of the current period
- Low - lowest price of the current period
- Volume - trading volume of the current period
- WVAD(previous) - previous indicator value

If High = Low (the range is zero), the value for that period is not added.

The indicator is cumulative -- values accumulate with each new period.

## See Also

[WAD](williams_accumulation_distribution.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
