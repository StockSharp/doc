# IMI

**índice de impulso intradía (IMI)** es un indicador técnico desarrollado por Tushar Chande que combina principios de fijación de precios intradía y el concepto RSI para medir el impulso intradiario.

Para utilizar el indicador, debe utilizar la clase [IntradayMomentumIndex](xref:StockSharp.Algo.Indicators.IntradayMomentumIndex).

## Descripción

El índice de impulso intradía (IMI) fue creado como una modificación del clásico índice de fuerza relativa (RSI), adaptado específicamente para analizar la dinámica del mercado intradiario. En lugar de utilizar precios de cierre secuenciales, como en el RSI tradicional, IMI compara el precio de cierre con el precio de apertura de cada período.

IMI evalúa con qué frecuencia y con qué fuerza el precio de cierre supera el precio de apertura (impulso positivo) o cae por debajo del precio de apertura (impulso negativo) durante un período determinado. Esto permite identificar la dirección predominante y la fuerza del movimiento intradiario.

El indicador es particularmente útil para:
- Determinar la dirección del mercado intradiario
- Identificar posibles puntos de reversión
- Determinar los niveles de sobrecompra y sobreventa
- Detectar divergencias entre precio e impulso

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 14)

## Cálculo

El cálculo de índice de impulso intradía implica los siguientes pasos:

1. Determinar el movimiento del precio intradía:
   ```
   ganancia = Close - Open, si Close > Open
   pérdida = Open - Close, si Close < Open
   ```

2. Calcule la suma de los movimientos positivos y negativos durante el período Length:
   ```
   suma de ganancias = suma de todas las ganancias durante el periodo Length
   suma de pérdidas = suma de todas las pérdidas durante el periodo Length
   ```

3. Calcule IMI usando una fórmula similar a RSI:
   ```
   IMI = 100 * (suma de ganancias / (suma de ganancias + suma de pérdidas))
   ```

Nota: Si (Ganancias Sum + Pérdidas Sum) es igual a cero, IMI se establece en 50 para evitar la división por cero.

## Interpretación

El índice de impulso intradía se interpreta de manera similar a RSI:

1. **Value Range**:
   - IMI oscila entre 0 y 100
   - Valores superiores a 50 indican predominio de un impulso intradiario positivo
   - Valores por debajo de 50 indican predominio del impulso intradiario negativo

2. **Niveles de sobrecompra y sobreventa**:
   - Los valores superiores a 70 generalmente se consideran indicadores de condiciones de sobrecompra en el mercado.
   - Los valores por debajo de 30 generalmente se consideran indicadores de condiciones de sobreventa del mercado.

3. **Cruces de línea central**:
   - Cruzar la línea 50 de abajo hacia arriba puede verse como una señal alcista.
   - Cruzar la línea 50 de arriba a abajo puede verse como una señal bajista.

4. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que IMI forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que IMI forma un máximo más bajo

5. **Failed Swings**:
   - Si IMI no puede alcanzar el nivel de sobrecompra durante una tendencia alcista, esto puede indicar debilidad de la tendencia.
   - Si IMI no puede alcanzar el nivel de sobreventa durante una tendencia a la baja, esto puede indicar debilidad de la tendencia.

6. **Confirmación de tendencia**:
   - Los valores sostenidos de IMI por encima de 50 confirman una tendencia al alza
   - Los valores sostenidos de IMI por debajo de 50 confirman una tendencia a la baja

![indicator_intraday_momentum_index](../../../../images/indicator_intraday_momentum_index.png)

## Véase también

[RSI](rsi.md)
[IntradayIntensityIndex](intraday_intensity_index.md)
[Momentum](momentum.md)
[RelativeMomentumIndex](relative_momentum_index.md)