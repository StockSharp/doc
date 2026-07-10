# MAC

**Moving Average Crossover (MAC)** es un indicador técnico que rastrea los cruces entre promedios móviles cortos y largos para identificar posibles puntos de entrada y salida del mercado.

Para utilizar el indicador, debe utilizar la clase [MovingAverageCrossover](xref:StockSharp.Algo.Indicators.MovingAverageCrossover).

## Descripción

El indicador Moving Average Crossover (MAC) es uno de los indicadores más utilizados y fáciles de entender en el análisis técnico. Se basa en el concepto de que cuando una media móvil de corto plazo cruza una media móvil de largo plazo, puede indicar un cambio de tendencia o un movimiento significativo de precios.

MAC utiliza dos medias móviles con períodos diferentes:
1. Media móvil corta (FastMA): refleja el movimiento reciente de precios
2. Media móvil larga (SlowMA): refleja el movimiento de precios a largo plazo

El indicador generalmente se representa como la diferencia entre las medias móviles cortas y largas, lo que permite una fácil identificación del momento de cruce (cuando el valor del indicador cruza la línea cero).

MAC se utiliza ampliamente tanto en estrategias de trading independientes como como parte de sistemas más complejos, como MACD (convergencia/divergencia de medias móviles).

## Parámetros

El indicador tiene los siguientes parámetros:
- **ShortPeriod** - período para la media móvil corta (valor predeterminado: 9)
- **LongPeriod** - período para la media móvil larga (valor predeterminado: 26)

## Cálculo

El cálculo del indicador Moving Average Crossover implica los siguientes pasos:

1. Calcule la media móvil corta:
   ```
   FastMA = SMA(Price, ShortPeriod)
   ```

2. Calcule la media móvil larga:
   ```
   SlowMA = SMA(Price, LongPeriod)
   ```

3. Calcule el valor MAC como la diferencia entre medias móviles cortas y largas:
   ```
   MAC = FastMA - SlowMA
   ```

donde:
- Price - precio (normalmente precio de cierre)
- SMA - media móvil simple
- ShortPeriod - período para la media móvil corta
- LongPeriod - período para la media móvil larga

Nota: También se pueden utilizar otros tipos de medias móviles como EMA (media móvil exponencial), WMA (media móvil ponderada), etc., en lugar de SMA.

## Interpretación

El indicador Moving Average Crossover se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - Cruzar la línea cero MAC de abajo hacia arriba (FastMA cruza SlowMA de abajo hacia arriba) genera una señal alcista, lo que indica un posible inicio de tendencia alcista.
   - Cruzar la línea cero MAC de arriba a abajo (FastMA cruza SlowMA de arriba a abajo) genera una señal bajista, lo que indica un posible inicio de tendencia bajista.

2. **Valor del indicador**:
   - El valor positivo de MAC indica que la media móvil corta está por encima de la media móvil larga, lo que a menudo se interpreta como un estado de mercado alcista.
   - El valor negativo de MAC indica que la media móvil corta está por debajo de la media móvil larga, lo que a menudo se interpreta como un estado de mercado bajista.

3. **Distancia entre medias móviles**:
   - La distancia creciente entre promedios móviles (aumento del valor absoluto MAC) indica fortalecimiento de la tendencia
   - La distancia decreciente entre los promedios móviles (valor absoluto MAC decreciente) puede indicar un debilitamiento de la tendencia y una posible reversión.

4. **False Signals**:
   - Durante los períodos de consolidación lateral, MAC puede generar múltiples señales falsas debido a frecuentes cruces de medias móviles
   - A menudo se utilizan indicadores o reglas adicionales para filtrar señales falsas (por ejemplo, lo que requiere que el precio sea por encima/por debajo en ambas medias móviles).

5. **Combinando con otros indicadores**:
   - MAC se utiliza a menudo en combinación con indicadores de impulso (RSI, estocástico) para confirmar señales
   - También se puede combinar con indicadores de tendencia y volatilidad para crear sistemas de trading más completos.

6. **Parameter Selection**:
   - Los períodos más cortos (por ejemplo, 5 y 20) son más sensibles y adecuados para operaciones a corto plazo.
   - Los períodos más largos (por ejemplo, 50 y 200) son menos sensibles y adecuados para operaciones a largo plazo.

![indicator_moving_average_crossover](../../../../images/indicator_moving_average_crossover.png)

## Véase también

[SMA](sma.md)
[EMA](ema.md)
[MACD](macd.md)
[MovingAverageRibbon](moving_average_ribbon.md)
