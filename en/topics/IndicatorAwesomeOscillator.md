# AO

The **Awesome Oscillator (AO)** is a classic technical indicator constructed by subtracting moving averages (SMA) with different periods.

To use the indicator, the [AwesomeOscillator](xref:StockSharp.Algo.Indicators.AwesomeOscillator) class should be used.
##### Calculation

The Awesome Oscillator histogram is a 34-period simple moving average constructed on the central values of bars (H+L) / 2, subtracted from a 5-period simple moving average on the central points (H+L) / 2. Thus, the slow moving average line is subtracted from the fast one, to get an idea of the strength of price movement and its further intentions.

MEDIAN PRICE = (HIGH + LOW) / 2  
AO = SMA (MEDIAN PRICE, 5) – SMA (MEDIAN PRICE, 34), where  
  
MEDIAN PRICE — median price  
HIGH — the highest price of the bar  
LOW — the lowest price of the bar  
SMA — simple moving average

The values are taken for the classic indicator, and in the settings, it is always possible to specify your own parameters.

![IndicatorAwesomeOscillator](../images/IndicatorAwesomeOscillator.png)

## See Also

[Bollinger Bands](IndicatorBollingerBands.md)
