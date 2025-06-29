# A/D

**Acceleration/Deceleration (A/D)** is an oscillator that was created by Bill Williams. It measures the acceleration and deceleration of the trend's momentum.

To use the indicator, the [Acceleration](xref:StockSharp.Algo.Indicators.Acceleration) class should be used.
##### Calculation
  
The A/D histogram is the difference between the value of the 5/34 histogram of driving force and the 5-period simple moving average taken from this histogram. The values are taken for the classic oscillator, and in the settings, it is always possible to specify your own parameters.

MEDIAN PRICE = (HIGH + LOW) / 2  
AO = SMA (MEDIAN PRICE, 5) - SMA (MEDIAN PRICE, 34)  
A/D = AO - SMA (AO, 5)  
  
where:  
  
MEDIAN PRICE — median price;  
HIGH — the highest price of the bar;  
LOW — the lowest price of the bar;  
SMA — simple moving average;  
AO — [Awesome Oscillator](ao.md) indicator.  

The parameters are set as the SMA periods values.

![IndicatorAcceleration](../../../../images/indicatoracceleration.png)

## See Also

[Alligator](alligator.md)
