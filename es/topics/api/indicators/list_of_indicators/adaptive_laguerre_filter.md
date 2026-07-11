# ALF

**filtro adaptativo de Laguerre (ALF)** es un indicador desarrollado para suavizar los datos de precios con un retraso mínimo, basado en los principios matemáticos del filtro de Laguerre.

Para utilizar el indicador, debe utilizar la clase [AdaptiveLaguerreFilter](xref:StockSharp.Algo.Indicators.AdaptiveLaguerreFilter).

## Descripción

El filtro adaptativo de Laguerre es una herramienta avanzada de filtrado de ruido del mercado. Proporciona una representación más fluida del movimiento de precios al tiempo que mantiene una respuesta rápida a los cambios de tendencia reales. Este filtro es particularmente útil para reducir el retraso que a menudo se encuentra en los indicadores de suavizado tradicionales.

La principal ventaja de ALF sobre las medias móviles clásicas radica en su capacidad para separar más eficazmente el ruido del mercado de los movimientos de precios genuinos, lo que lo convierte en una herramienta valiosa para los operadores que buscan reducir las señales falsas.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Gamma** - coeficiente de filtrado (normalmente en el rango de 0,1 a 0,9)

El parámetro Gamma determina el grado de suavizado: los valores más bajos crean una línea más suave con más retraso, mientras que los valores más altos dan como resultado menos suavizado pero una respuesta más rápida a los cambios de precios.

## Cálculo

El filtro adaptativo de Laguerre se basa en polinomios de Laguerre y representa un sistema de filtrado de respuesta de impulso finito (FIR). El cálculo utiliza las siguientes fórmulas:

1. Se calculan los valores intermedios L0, L1, L2 y L3:
   ```
   L0(t) = (1 - γ) * price(t) + γ * L0(t-1)
   L1(t) = -γ * L0(t) + L0(t-1) + γ * L1(t-1)
   L2(t) = -γ * L1(t) + L1(t-1) + γ * L2(t-1)
   L3(t) = -γ * L2(t) + L2(t-1) + γ * L3(t-1)
   ```

2. El valor final ALF se calcula como el promedio:
   ```
   ALF = (L0 + L1 + L2 + L3) / 4
   ```

donde:
- γ (gamma) - coeficiente de filtrado
- precio(t) - precio actual
- L0, L1, L2, L3 - valores de filtro intermedios

![indicator_adaptive_laguerre_filter](../../../../images/indicator_adaptive_laguerre_filter.png)

## Véase también

[LaguerreRSI](laguerre_rsi.md)
[ZLEMA](zero_lag_exponential_moving_average.md)