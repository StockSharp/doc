# Parabolic SAR

**Parabolic SAR (SAR)** - A trend indicator that indicates price stop and reversal points, as well as the trend direction.

To use the indicator, the [ParabolicSar](xref:StockSharp.Algo.Indicators.ParabolicSar) class should be used.
##### Indicator Calculation
  
The price of the indicator point (SAR) for the next period (candle) is calculated using the following formulas:
  
SAR(n+1) = SAR(n) + a * (high � SAR(n)), for an uptrend;  
SAR(n+1) = SAR(n) + a * (low � SAR(n)), for a downtrend, where:

SAR(n+1) � price for the period n+1;  
SAR(n) � price for the period n;  
high and low � new maximum and minimum respectively (extremes). They are considered for the time interval between the activation of the previous indicator signal and the current moment;
  
a � acceleration factor.
  
The acceleration factor is a floating coefficient, characterized by minimum, maximum values, and a step of change.
  
The factor takes a minimum value equal to one step at the reversal point, and as soon as the price reaches a new extreme value according to the trend (high or low), the factor is increased by a step. When the factor reaches its maximum value, its growth is paused.
  
![IndicatorParabolicSar](../../../../images/indicatorparabolicsar.png)

## See Also

[Peak](peak.md)
