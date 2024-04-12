# Alligator

The **Alligator** indicator consists of a group of three moving averages. These moving averages have different periods and are also shifted forward on the chart.

To use the indicator, the [Alligator](xref:StockSharp.Algo.Indicators.Alligator) class should be used.
##### Features of the "Alligator" indicator
  
The indicator consists of three differently colored lines:
  
- Blue, named the jaw, with a calculation period of 13 and a shift of 8 bars. When it is located under the price curve, it indicates a potential upward price movement. If the "jaw" rises above it, a decline is expected.

- Red, called the alligator's teeth, with a period of 8 and a shift of 5 bars. It belongs to the fast averages and shows the price behavior in the hourly range.
  
- Green, called the lips, with a period of 5 and a shift of 3 bars. It analyzes twelve-minute market trends.

The values are taken for the classic indicator, and in the settings, it is always possible to specify your own parameters.

![IndicatorAlligator](../../../../images/indicatoralligator.png)

## See Also

[ADX](adx.md)
