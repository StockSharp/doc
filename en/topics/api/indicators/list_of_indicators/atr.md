# ATR

**Average True Range (ATR)** is an indicator that shows the level of current volatility.

To use the indicator, the [AverageTrueRange](xref:StockSharp.Algo.Indicators.AverageTrueRange) class should be used.
##### Calculation of the Indicator
  
The calculation of the indicator begins with determining the True Range (TR), which is calculated as the maximum of the following three values:  
- the difference between the current maximum and minimum;  
- the difference between the current maximum and the previous closing price (absolute value);  
- the difference between the current minimum and the previous closing price (absolute value).  

TRt = max(High(t)-Low(t); High(t) - Close(t-1); Close(t-1)-Low(t))  

The absolute value is used to ensure positive values, as we are interested in the distance between two points, not the direction of price movement.  
  
Based on this indicator, ATR is calculated. It has only one parameter - the period N. By default, a 14-period indicator is used, but it can be adjusted to fit your own strategy. Here is what the formula looks like (this is one of the forms of the exponential moving average)  
  
ATR(t) = ((ATR(t-1) x (N-1)) + TR(t)) / N  

![IndicatorAverageTrueRange](../../../../images/indicatoraveragetruerange.png)

## See Also

[AO](ao.md)