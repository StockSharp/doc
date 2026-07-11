# FRAMA

**Media móvil adaptativa fractal (FRAMA)** es un indicador técnico desarrollado por John Ehlers que adapta la tasa de reacción a los cambios de precios en función de la dimensión fractal del mercado.

Para utilizar el indicador, debe utilizar la clase [FractalAdaptiveMovingAverage](xref:StockSharp.Algo.Indicators.FractalAdaptiveMovingAverage).

## Descripción

La media móvil adaptativa fractal (FRAMA) es un tipo avanzado de media móvil exponencial (EMA) que ajusta automáticamente su sensibilidad a los cambios de precios en función de la dimensión fractal del mercado. El indicador fue desarrollado por John Ehlers y presentado en la revista Technical Analysis of Stocks & Commodities en octubre de 2000.

FRAMA utiliza el concepto de geometría fractal para analizar la estructura del mercado. Determina qué tan "fractal" o caótico es el mercado actual y, en base a esto, ajusta la tasa de respuesta del indicador:

- En condiciones de mercado de tendencia (menos fractales), FRAMA responde rápidamente a los cambios de precios, similar a un EMA corto.
- En condiciones de mercado laterales (más fractales), FRAMA responde más lentamente, similar a un EMA largo.

Esto permite que FRAMA responda más rápido a movimientos de precios significativos e ignore el ruido del mercado, lo que lo hace más efectivo en comparación con los promedios móviles tradicionales.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 10-20)

## Cálculo

El cálculo de FRAMA implica varios pasos:

1. Calcule la dimensión fractal (D) basándose en la relación logarítmica entre la duración del precio alto y bajo y el número de períodos:
   ```
   N1 = High(1...Length/2) - Low(1...Length/2)
   N2 = High(Length/2+1...Length) - Low(Length/2+1...Length)
   N3 = High(1...Length) - Low(1...Length)
   
   D = (log(N1 + N2) - log(N3)) / log(2)
   ```

2. Convierta la dimensión fractal en factor alfa para un suavizado exponencial:
   ```
   Smoothing Factor = exp(-4.6 * (D - 1))
   Alpha = Smoothing Factor * Smoothing Factor
   ```

3. Aplique el factor alfa al precio actual y al valor FRAMA anterior:
   ```
   FRAMA = Alpha * Price + (1 - Alpha) * FRAMA[previous]
   ```

donde:
- High - precio máximo para el período
- Low - precio mínimo para el período
- log - logaritmo natural

## Interpretación

FRAMA se puede interpretar de manera similar a otras medias móviles, pero teniendo en cuenta su naturaleza adaptativa:

1. **FRAMA Dirección**:
   - Hacia arriba FRAMA indica una tendencia alcista
   - Downward FRAMA indica una tendencia a la baja

2. **Cruces con precio**:
   - Cuando el precio cruza FRAMA de abajo hacia arriba, puede verse como una señal alcista.
   - Cuando el precio cruza FRAMA de arriba a abajo, puede verse como una señal bajista.

3. **Múltiples cruces FRAMA**:
   - El cruce de un FRAMA corto con un FRAMA largo de abajo hacia arriba puede indicar el inicio de una tendencia alcista
   - El cruce de un FRAMA corto con un FRAMA largo de arriba a abajo puede indicar el inicio de una tendencia a la baja.

4. **FRAMA Ángulo de pendiente**:
   - Un ángulo de pendiente pronunciado indica una fuerte tendencia
   - El ángulo de pendiente poco profundo indica una tendencia débil
   - El movimiento horizontal indica una tendencia lateral.

5. **Filtrado de señal**:
   - Debido a su naturaleza adaptativa, FRAMA crea menos señales falsas que las medias móviles tradicionales.
   - Cuanto más corto sea el período FRAMA, más sensible será el indicador a los cambios de precios.

6. **Niveles de soporte y resistencia**:
   - FRAMA puede servir como nivel de soporte dinámico en una tendencia alcista
   - FRAMA puede servir como nivel de resistencia dinámica en una tendencia a la baja

![indicator_fractal_adaptive_moving_average](../../../../images/indicator_fractal_adaptive_moving_average.png)

## Véase también

[EMA](ema.md)
[KAMA](kama.md)
[VIDYA](vidya.md)
