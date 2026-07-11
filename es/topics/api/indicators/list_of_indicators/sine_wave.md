# SW

**Sine Wave (SW)** es un indicador técnico que utiliza la función matemática seno para identificar patrones cíclicos en el movimiento de precios. El indicador tiene como objetivo identificar y predecir oscilaciones periódicas del mercado.

Para utilizar el indicador, debe utilizar la clase [SineWave](xref:StockSharp.Algo.Indicators.SineWave).

## Descripción

El indicador Sine Wave se basa en la idea de que los movimientos del mercado tienen una naturaleza cíclica y pueden modelarse mediante funciones sinusoidales. Este indicador es particularmente útil en mercados que se mueven en un rango lateral o tienen fluctuaciones cíclicas predecibles.

Características clave del indicador:
- Ayuda a identificar posibles puntos de reversión del mercado.
- Permite determinar la posición actual en el ciclo.
- Se puede utilizar para pronosticar movimientos futuros de precios.

Señales indicadoras:
- Compra potencial cuando la línea de onda sinusoidal alcanza un mínimo y comienza a girar hacia arriba
- Venta potencial cuando la línea alcanza un máximo y comienza a girar hacia abajo

## Parámetros

- **Length** - período del ciclo de onda sinusoidal, que define la duración del ciclo en las barras de precios.

## Cálculo

El cálculo del indicador Sine Wave se basa en el uso de la función seno y la determinación del ciclo dominante en el movimiento de precios:

1. Determinación del ciclo dominante mediante análisis espectral u otro método de identificación del ciclo.

2. Aplicando la función seno para modelar el ciclo identificado:
   ```
   SineWave(t) = A * sin(2π * t / Length + φ)
   ```
   donde:
   - A - amplitud (altura de onda)
   - t - hora actual o barra
   - Length - duración del ciclo
   - φ - cambio de fase para alinear la onda sinusoidal con el ciclo de precios real

3. Además, se puede calcular un indicador adelantado que adelanta la onda sinusoidal principal en un cuarto de ciclo:
   ```
   Lead(t) = A * sin(2π * t / Length + φ + π/2)
   ```

El indicador puede incluir componentes adicionales como una línea de tendencia o un filtro para mejorar la precisión de la señal.

![IndicatorSineWave](../../../../images/indicator_sine_wave.png)

## Véase también

[Ciclo de tendencia de Schaff](schaff_trend_cycle.md)