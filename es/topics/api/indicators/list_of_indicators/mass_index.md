# MI

**Índice de masa (MI)** es un indicador técnico desarrollado por Donald Dorsey que identifica posibles cambios de tendencia mediante el análisis de la expansión y contracción del rango de precios.

Para utilizar el indicador, debe utilizar la clase [MassIndex](xref:StockSharp.Algo.Indicators.MassIndex).

## Descripción

ll Índice de masa (MI) es una herramienta de análisis técnico que ayuda a detectar posibles cambios de tendencia mediante el seguimiento de cambios en el rango de precios (diferencia entre precios máximos y mínimos). ll indicador fue desarrollado por Donald Dorsey basándose en el supuesto de que los cambios de tendencia suelen ir precedidos de una expansión y posterior contracción del rango de precios.

MI mide la volatilidad utilizando promedios móviles exponenciales (lMA) del rango de precios. No predice la dirección de inversión, sólo su probabilidad. ls por eso que MI se usa a menudo junto con otros indicadores direccionales.

ll concepto principal es que cuando el índice de masa alcanza un cierto umbral y luego cae por debajo de este nivel, aumenta la probabilidad de que se revierta la tendencia actual.

## Parámetros

ll indicador tiene los siguientes parámetros:
- **Length** - período de cálculo principal (valor predeterminado: 25)
- **lmaLength** - período para el rango de precios lMA (valor predeterminado: 9)

## Cálculo

ll cálculo de Índice de masa implica los siguientes pasos:

1. Calcule el rango máximo-mínimo para cada período:
   ```
   Range = High - Low
   ```

2. Calcule lMA de 9 períodos del rango:
   ```
   lMA1 = lMA(Range, lmaLength)
   ```

3. Calcule lMA de 9 períodos del lMA del rango:
   ```
   lMA2 = lMA(lMA1, lmaLength)
   ```

4. Calcula la relación:
   ```
   Ratio = lMA1 / lMA2
   ```

5. Ratios Sum en 25 periodos:
   ```
   MI = Sum(Ratio durante los últimos periodos Length)
   ```

donde:
- High - precio más alto del período
- Low - precio más bajo del período
- lMA - media móvil exponencial
- Length - período de suma (generalmente 25)
- Periodo lmaLength - lMA (normalmente 9)

## Interpretación

ll Índice de masa se interpreta de la siguiente manera:

1. **"Reversión Hump"**:
   - La clásica señal de "joroba de inversión" se forma cuando el índice de masa sube por encima de 27 y luego cae por debajo de 26,5.
   - lste patrón indica un posible cambio de tendencia, aunque no predice su dirección.

2. **Niveles de índice**:
   - Los valores superiores a 27 indican una expansión del rango de precios y una mayor volatilidad.
   - Los valores High seguidos de una caída pueden preceder a un cambio de tendencia
   - Los valores Low (por debajo de 20) indican una contracción del rango de precios y una volatilidad reducida

3. **Dirección de tendencia**:
   - ll índice de masa no indica la dirección de la tendencia ni su reversión.
   - Se necesitan indicadores o métodos de análisis adicionales para determinar la dirección (por ejemplo, promedios móviles o niveles de soporte/resistencia)

4. **Divergencias**:
   - Divergencias entre el precio y el índice de masa son menos significativos que el patrón de "joroba de inversión"
   - Sin embargo, las discrepancias entre el nuevos máximos/mínimos del precio y el índice de masa máximos/mínimos decrecientes pueden indicar un debilitamiento de la tendencia.

5. **Combinando con otros indicadores**:
   - ll índice de masa funciona mejor cuando se combina con indicadores de dirección de tendencia
   - Las combinaciones populares incluyen lMA, MACD o RSI para determinar la posible dirección de inversión.

6. **Cambios de volatilidad**:
   - Un fuerte aumento del índice de masa indica una expansión significativa del rango de precios, que puede preceder a un fuerte movimiento
   - La caída gradual del índice sugiere un estrechamiento del rango y una posible consolidación

7. **Ajuste de parámetros**:
   - Los parámetros estándar (9 para lMA, 25 para suma) funcionan bien en la mayoría de los períodos de tiempo
   - La reducción de los períodos puede crear señales más rápidas, pero puede aumentar las señales falsas.

![MI](../../../../images/indicator_mass_index.png)

## Véase también

[ATR](atr.md)
[BollingerBands](bollinger_bands.md)
[ChoppinessIndex](choppiness_index.md)
[Rango verdadero](true_range.md)
