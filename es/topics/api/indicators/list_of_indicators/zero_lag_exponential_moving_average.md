# ZLEMA

**Zero Lag Exponential Moving Average (ZLEMA)** es una versión modificada de la media móvil exponencial (EMA), desarrollada por John Ehlers. ZLEMA está diseñado para eliminar o reducir significativamente el retraso inherente a las medias móviles tradicionales.

Para utilizar el indicador, debe utilizar la clase [ZeroLagExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ZeroLagExponentialMovingAverage).

## Descripción

El Zero Lag Exponential Moving Average (ZLEMA) fue creado para resolver el problema principal de la mayoría de los promedios móviles: el retraso de la señal. Los promedios móviles tradicionales van por detrás de los movimientos de precios debido a la ventana de tiempo utilizada para su cálculo. ZLEMA minimiza este retraso mediante el uso de un mecanismo de corrección basado en la diferencia entre el precio actual y el precio del pasado.

Principales ventajas de ZLEMA:
- Reacción más rápida a los cambios de precios
- Menos retraso en comparación con las medias móviles tradicionales
- Preservación del efecto suavizante característico de EMA

ZLEMA se puede utilizar para:
- Determinar la dirección de la tendencia
- Encontrar puntos de entrada y salida
- Identificar niveles de soporte y resistencia.
- Creación de sistemas de trading basados en cruces.

## Parámetros

- **Length**: período de cálculo que determina el grado de suavizado (similar al período en EMA).

## Cálculo

El cálculo de ZLEMA se basa en la eliminación del retraso mediante la previsión e incluye los siguientes pasos:

1. Calcule el retraso como la mitad del período:
   ```
   lag = (Length - 1) / 2
   ```

2. Calcule el precio "sin tendencia":
   ```
   detrendedPrice = 2 * Price - Price[lag]
   ```
   Este es un paso clave que permite "mirar hacia adelante" y eliminar retrasos.

3. Aplique suavizado exponencial al precio sin tendencia:
   ```
   k = 2 / (Length + 1)
   ZLEMA = k * detrendedPrice + (1 - k) * ZLEMA[previous]
   ```

El resultado es una media móvil que sigue el precio mucho más cerca que un EMA normal con el mismo período, manteniendo al mismo tiempo el efecto de suavizado.

![IndicatorZeroLagExponentialMovingAverage](../../../../images/indicator_zero_lag_exponential_moving_average.png)

## Véase también

[EMA](ema.md)
[DEMA](dema.md)
[TEMA](tema.md)
[T3MA](t3_moving_average.md)