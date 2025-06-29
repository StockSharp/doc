# Momentum

The **Momentum** indicator measures the magnitude of the price change of a financial instrument over a certain period. It indicates overbought and oversold moments when the curve reaches maximum or minimum values. Adding a smoothed moving average to the indicator improves the interpretation of trend changes.

To use the indicator, the [Momentum](xref:StockSharp.Algo.Indicators.Momentum) class should be used.
##### Calculation
  
Momentum is defined as the ratio of today's price to the price n periods ago:
 
MOMENTUM = CLOSE(i) / CLOSE(i - n) * 100  

where:  
CLOSE(i) — the closing price of the current bar;  
CLOSE(i - n) — the closing price of n bars back.  


![IndicatorMomentum](../../../../images/indicatormomentum.png)

## See Also

[Money Flow Index](money_flow_index.md)
