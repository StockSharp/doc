# FOSC

**Oscilador de pronóstico (FOSC)** es un indicador técnico que mide la desviación del precio de su valor previsto obtenido mediante regresión lineal, representando esta desviación como un porcentaje.

Para utilizar el indicador, debe utilizar la clase [ForecastOscillator](xref:StockSharp.Algo.Indicators.ForecastOscillator).

## Descripción

El Oscilador de pronóstico (FOSC) se basa en regresión lineal y está diseñado para medir el grado de desviación del precio actual con respecto a su valor previsto. Ayuda a los operadores a evaluar en qué medida el precio actual corresponde a la tendencia esperada o se desvía de ella.

El indicador calcula una línea de tendencia mediante regresión lineal durante un período específico y luego compara el precio de cierre real con el valor previsto en esta línea. La diferencia se expresa como porcentaje, lo que convierte a FOSC en un oscilador que fluctúa alrededor de la línea cero.

El Oscilador de pronóstico es particularmente útil para:
- Determinar el grado de alineación de precios con la tendencia esperada.
- Identificar posibles puntos de reversión
- Detectar desviaciones extremas de precios respecto de la tendencia
- Identificar períodos en los que el precio se mueve más rápido o más lento de lo esperado

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período para el cálculo de regresión lineal (valor predeterminado: 14)

## Cálculo

El cálculo de Oscilador de pronóstico implica los siguientes pasos:

1. Calcule la línea de pronóstico mediante regresión lineal durante el período especificado:
   ```
   Pronóstico = línea de regresión lineal(Close, Length)
   ```

2. Calcule el oscilador como una relación porcentual entre el precio actual y el valor previsto:
   ```
   FOSC = ((Close - Pronóstico) / Pronóstico) * 100
   ```

donde:
- Close - precio de cierre actual
- Pronóstico: valor previsto obtenido mediante regresión lineal
- Length - período para el cálculo de regresión lineal

## Interpretación

El Oscilador de pronóstico se interpreta de la siguiente manera:

1. **Desviación de cero**:
   - Los valores positivos (FOSC > 0) indican que el precio actual está por encima del valor previsto, lo que puede sugerir un movimiento alcista más fuerte de lo esperado.
   - Los valores negativos (FOSC < 0) indican que el precio actual está por debajo del valor previsto, lo que puede sugerir un movimiento a la baja más fuerte de lo esperado.

2. **Valores extremos**:
   - Valores positivos muy altos pueden indicar condiciones de sobrecompra del mercado en relación con la tendencia.
   - Valores negativos muy bajos pueden indicar condiciones de sobreventa del mercado en relación con la tendencia.

3. **Retorno a cero**:
   - El movimiento de FOSC desde valores extremos hacia cero puede indicar un posible retorno del precio a su línea de tendencia

4. **Cruces de línea cero**:
   - El cruce de la línea cero de abajo hacia arriba puede verse como una señal alcista
   - El cruce de la línea cero de arriba a abajo puede verse como una señal bajista

5. **Divergencias**:
   - Divergencia alcista (el precio forma un nuevo mínimo, mientras que FOSC forma un mínimo más alto) puede indicar una posible reversión al alza
   - Divergencia bajista (el precio forma un nuevo máximo, mientras que FOSC forma un máximo más bajo) puede indicar una posible reversión a la baja

6. **Confirmación de tendencia**:
   - Si FOSC se mueve en la misma dirección que el precio, esto confirma la fuerza de la tendencia actual.
   - Si FOSC se mueve en la dirección opuesta al precio, esto puede indicar un debilitamiento de la tendencia actual.

![Gráfico del indicador FOSC](../../../../images/indicator_forecast_oscillator.png)

## Véase también

[LinearRegression](lrc.md)
[StandardError](standard_error.md)
[DisparityIndex](disparity_index.md)
