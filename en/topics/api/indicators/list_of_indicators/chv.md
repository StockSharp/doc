# CHV

**Chaikin Volatility (CHV)** indicates the difference between the maximum of purchases and the minimum of sales over a period of time. The indicator allows for qualitative analysis of price changes and the range width between the price maximum and minimum. CHV does not consider price gaps in its calculation, which can be considered a drawback to some extent.

To use the indicator, the [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility) class should be used.
##### Calculation

The calculation of the Chaikin Volatility indicator begins with determining the spread � the difference between the maximum and minimum of the current bar. The obtained result is smoothed with an exponential moving average over the corresponding period. Volatility is calculated by the formula:

CHV = (EMA(H-L(i), n) � EMA(H-L(i-n), n)) / EMA(H-L(i-n), n) x 100.

In this case, H-L(i) represents the price difference on the current bar, and H-L(i-n) � the price difference on a bar that occurred n periods earlier. Thus, we have the ability to visually see how the price has changed over time.

Main parameters of the indicator:

- **ROCPeriod** � the period number relative to which the calculation will be performed. Initially set to 5.
- **SmoothPeriod** � the period of the moving average. By default, it is set to 32.

![IndicatorChaikinVolatility](../../../../images/indicatorchaikinvolatility.png)

## See Also

[CMO](cmo.md)