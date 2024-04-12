# %R

**Williams %R (%R, Williams� Percent Range)** is a momentum indicator that fluctuates between 0 and -100 and displays overbought and oversold levels.

To use the indicator, the [WilliamsR](xref:StockSharp.Algo.Indicators.WilliamsR) class should be used.
##### Calculation
  
The formula for calculating the Williams� Percent Range indicator is similar to that used for calculating the Stochastic Oscillator:

%R = - (MAX(HIGH(i - n)) - CLOSE(i)) / (MAX(HIGH(i - n)) - MIN(LOW(i - n))) * 100  
  
where:
  
CLOSE(i) - today's closing price;  
MAX(HIGH(i - n)) - the highest maximum of the past n periods;  
MIN(LOW(i - n)) - the lowest minimum of the past n periods.  

The value of n is set as the indicator parameter.

![IndicatorWilliamsR](../../../../images/indicatorwilliamsr.png)

## See Also

[ZigZag](zigzag.md)
