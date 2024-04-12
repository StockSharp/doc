# Envelope

**Envelope** is an indicator that forms a channel created by offsetting a moving average by a specific value. The method of constructing the indicator exactly replicates the construction of Bollinger Bands, except for the calculation of the distance that the outer lines are from the average. If Bollinger Bands use standard deviation for this calculation, in **Envelope**, this distance is manually set in the settings.
The parameters set are the period of the moving average and the size of the deviation.

To use the indicator, the [Envelope](xref:StockSharp.Algo.Indicators.Envelope) class should be used.

![IndicatorEnvelope](../../../../images/indicatorenvelope.png)

## See Also

[EMA](ema.md)