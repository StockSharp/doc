# HVR

**ratio de volatilidad histórica (HVR)** es un indicador técnico que compara la volatilidad histórica a corto plazo con la volatilidad histórica a largo plazo para evaluar los cambios en la actividad del mercado.

Para utilizar el indicador, debe utilizar la clase [HistoricalVolatilityRatio](xref:StockSharp.Algo.Indicators.HistoricalVolatilityRatio).

## Descripción

El ratio de volatilidad histórica (HVR) es un indicador de volatilidad relativa que compara la volatilidad a corto plazo con la volatilidad del mercado a largo plazo. El indicador ayuda a determinar si la volatilidad actual está aumentando o disminuyendo en relación con su nivel histórico.

HVR se calcula como la relación entre la volatilidad histórica a corto plazo y la volatilidad histórica a largo plazo. Los valores superiores a 1,0 indican que la volatilidad actual (a corto plazo) es mayor que la volatilidad a largo plazo, lo que puede indicar una mayor actividad del mercado o un posible cambio de tendencia.

El indicador es particularmente útil para:
- Identificar períodos de alta y baja volatilidad
- Determinación de posibles puntos de inversión de tendencia
- Adaptar las estrategias de trading a las condiciones actuales del mercado.
- Evaluar el riesgo de mercado y establecer tamaños de posición adecuados

## Parámetros

El indicador tiene los siguientes parámetros:
- **ShortPeriod** - período para calcular la volatilidad a corto plazo (valor predeterminado: 5)
- **LongPeriod** - período para calcular la volatilidad a largo plazo (valor predeterminado: 20)

## Cálculo

El cálculo de ratio de volatilidad histórica implica los siguientes pasos:

1. Calcule la volatilidad histórica a corto plazo:
   ```
   Volatilidad a corto plazo = Desviación estándar de los rendimientos logarítmicos durante ShortPeriod * Sqrt(Días de trading por año)
   ```

2. Calcule la volatilidad histórica a largo plazo:
   ```
   Volatilidad a largo plazo = Desviación estándar de los rendimientos logarítmicos durante LongPeriod * Sqrt(Días de trading por año)
   ```

3. Calcule HVR como la relación entre la volatilidad a corto plazo y la volatilidad a largo plazo:
   ```
   HVR = Volatilidad a corto plazo / Volatilidad a largo plazo
   ```

donde:
- Devoluciones de registros: rentabilidades logarítmicas (ln(Price[i] / Price[i-1]))
- Desviación estándar - desviación estándar
- Días de negociación por año: número de días de negociación en un año (normalmente 252 para los mercados de valores)
- ShortPeriod - breve período para el cálculo de la volatilidad
- LongPeriod - largo período para el cálculo de la volatilidad

## Interpretación

El ratio de volatilidad histórica se puede interpretar de la siguiente manera:

1. **Level 1.0**:
   - HVR = 1,0 significa que la volatilidad a corto plazo es igual a la volatilidad a largo plazo
   - HVR > 1,0 indica que la volatilidad a corto plazo es mayor que la volatilidad a largo plazo
   - HVR <1,0 indica que la volatilidad a corto plazo es menor que la volatilidad a largo plazo

2. **Valores extremos**:
   - Valores muy altos de HVR (por ejemplo, > 2,0) pueden indicar un fuerte aumento de la volatilidad, que a menudo ocurre durante pánicos en el mercado o movimientos fuertes.
   - Valores muy bajos de HVR (por ejemplo, < 0,5) pueden indicar un período de compresión de la volatilidad, que a menudo precede a movimientos fuertes

3. **Tendencias de HVR**:
   - El aumento de HVR indica un aumento en la volatilidad actual
   - La caída de HVR indica una disminución en la volatilidad actual

4. **Estrategias de trading**:
   - Cuando HVR es alto, puede ser apropiado utilizar estrategias basadas en rupturas
   - Cuando HVR es bajo, las estrategias de trading de reversión a la media o de rango pueden ser más adecuadas

5. **Gestión del riesgo**:
   - Los valores High HVR pueden indicar la necesidad de reducir el tamaño de las posiciones debido al aumento de la volatilidad
   - Los valores Low HVR pueden permitir mayores tamaños de posición debido a la reducción de la volatilidad

6. **Reversiones potenciales**:
   - Los valores extremos de HVR a menudo preceden a movimientos de precios significativos
   - Un fuerte aumento de HVR después de un período de baja volatilidad puede indicar el inicio de una nueva tendencia

![HVR](../../../../images/indicator_historical_volatility_ratio.png)

## Véase también

[ATR](atr.md)
[StandardDeviation](standard_deviation.md)
[ChoppinessIndex](choppiness_index.md)
