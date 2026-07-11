# CHV

**volatilidad de Chaikin (CHV)** indica la diferencia entre el máximo de compras y el mínimo de ventas durante un período de tiempo. El indicador permite el análisis cualitativo de los cambios de precios y el ancho del rango entre el precio máximo y mínimo. CHV no considera las diferencias de precios en su cálculo, lo que puede considerarse un inconveniente hasta cierto punto.

Para utilizar el indicador, se debe utilizar la clase [ChaikinVolatility](xref:StockSharp.Algo.Indicators.ChaikinVolatility).
##### Cálculo

El cálculo del indicador volatilidad de Chaikin comienza con la determinación del spread, la diferencia entre el máximo y el mínimo de la barra actual. El resultado obtenido se suaviza con una media móvil exponencial durante el período correspondiente. La volatilidad se calcula mediante la fórmula:

CHV = (EMA(H-L(i), n) — EMA(H-L(i-n), n)) / EMA(H-L(i-n), n) x 100.

En este caso, H-L(i) representa la diferencia de precio en la barra actual y H-L(i-n) es la diferencia de precio en una barra que ocurrió n períodos antes. Así, tenemos la capacidad de ver visualmente cómo ha cambiado el precio a lo largo del tiempo.

Principales parámetros del indicador:

- **ROCPeriod** — el número de período relativo al cual se realizará el cálculo. Inicialmente fijado en 5.
- **SmoothPeriod** — el período de la media móvil. Por defecto, está configurado en 32.

![IndicatorChaikinVolatility](../../../../images/indicatorchaikinvolatility.png)

## Véase también

[CMO](cmo.md)