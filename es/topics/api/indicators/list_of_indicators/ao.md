# AO

El **oscilador asombroso (AO)** es un indicador técnico clásico construido restando promedios móviles (SMA) con diferentes períodos.

Para utilizar el indicador, se debe utilizar la clase [AwesomeOscillator](xref:StockSharp.Algo.Indicators.AwesomeOscillator).
##### Cálculo

El histograma oscilador asombroso es una media móvil simple de 34 períodos construida sobre los valores centrales de las barras (H+L) / 2, restada de una media móvil simple de 5 períodos en los puntos centrales (H+L) / 2. Por lo tanto, la línea de media móvil lenta se resta de la rápida, para tener una idea de la fuerza del movimiento de precios y sus intenciones futuras.

PRECIO MEDIO = (ALTO + BAJO) / 2
AO = SMA (PRECIO MEDIO, 5) — SMA (PRECIO MEDIO, 34), donde

PRECIO MEDIO — precio medio
ALTO — el precio más alto de la barra
BAJO — el precio más bajo de la barra
SMA — media móvil simple

Los valores se toman para el indicador clásico y en la configuración siempre es posible especificar sus propios parámetros.

![IndicatorAwesomeOscillator](../../../../images/indicatorawesomeoscillator.png)

## Véase también

[bandas de Bollinger](bollinger_bands.md)
