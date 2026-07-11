# Envelope

**Envelope** es un indicador que forma un canal creado compensando una media móvil con un valor específico. El método de construcción del indicador replica exactamente la construcción de bandas de Bollinger, excepto por el cálculo de la distancia entre las líneas exteriores y el promedio. Si bandas de Bollinger usa la desviación estándar para este cálculo, en **Envelope**, esta distancia se establece manualmente en la configuración.
Los parámetros establecidos son el período de la media móvil y el tamaño de la desviación.

Para utilizar el indicador, se debe utilizar la clase [Envelope](xref:StockSharp.Algo.Indicators.Envelope).

![IndicatorEnvelope](../../../../images/indicatorenvelope.png)

## Véase también

[EMA](ema.md)