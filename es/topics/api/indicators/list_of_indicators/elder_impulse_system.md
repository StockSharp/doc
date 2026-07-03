# EIS

**Elder Impulse System (EIS)** es un indicador técnico desarrollado por el Dr. Alexander Elder que combina un indicador de tendencia y un oscilador de impulso para determinar la dirección y la fuerza del movimiento del mercado.

Para utilizar el indicador, debe utilizar la clase [ElderImpulseSystem](xref:StockSharp.Algo.Indicators.ElderImpulseSystem).

## Descripción

El Elder Impulse System (EIS) es una herramienta sencilla pero potente para visualizar el impulso del mercado. Combina dos indicadores:
1. **Media móvil exponencial (EMA)** - para determinar la dirección de la tendencia
2. **MACD Histogram** - para medir la fuerza y el impulso del movimiento de precios

EIS clasifica cada vela en el gráfico de precios en una de tres categorías (generalmente indicadas por diferentes colores):
- **Green (strong bullish impulse)** - cuando ambos indicadores están aumentando
- **Red (strong bearish impulse)** - cuando ambos indicadores están cayendo
- **Azul o neutral (sin impulso claro)** - cuando los indicadores se mueven en direcciones opuestas

EIS es particularmente útil para:
- Determinación visual rápida de la dirección y la fuerza de la tendencia
- Identificar puntos de entrada y salida en la dirección de la tendencia principal.
- Identificar posibles puntos de reversión
- Filtrar señales falsas

## Cálculo

El cálculo de Elder Impulse System implica los siguientes pasos:

1. Calcule la media móvil exponencial de 13 períodos (EMA):
   ```
   EMA = EMA(Close, 13)
   ```

2. Calcule MACD Histogram (valores estándar: 12, 26, 9):
   ```
   MACD Line = EMA(Close, 12) - EMA(Close, 26)
   Signal Line = EMA(MACD Line, 9)
   MACD Histogram = MACD Line - Signal Line
   ```

3. Determine la clasificación de color de la vela actual:
   ```
   If EMA[current] > EMA[previous] AND MACD Histogram[current] > MACD Histogram[previous], then Green (Bullish Impulse)
   If EMA[current] < EMA[previous] AND MACD Histogram[current] < MACD Histogram[previous], then Red (Bearish Impulse)
   Otherwise Blue (No Impulse)
   ```

## Interpretación

El Elder Impulse System se interpreta de la siguiente manera:

1. **Green Candles (strong bullish impulse)**:
   - Indicar un fuerte impulso alcista
   - El mejor momento para comprar o mantener posiciones largas
   - Una serie de velas verdes indica una fuerte tendencia alcista

2. **Red Candles (strong bearish impulse)**:
   - Indica un fuerte impulso a la baja
   - Mejor momento para vender o mantener posiciones cortas
   - Una serie de velas rojas indica una fuerte tendencia a la baja.

3. **Blue Candles (no clear impulse)**:
   - Indicar incertidumbre o consolidación.
   - Signal una posible desaceleración o cambio de tendencia
   - A menudo aparecen durante períodos de consolidación o antes de un cambio de tendencia.

4. **Estrategias de trading**:
   - Compre cuando las velas cambien de color de azul a verde
   - Vender cuando las velas cambien de color de azul a rojo.
   - Close posiciones largas cuando las velas cambian de color de verde a cualquier otro
   - Close posiciones cortas cuando las velas cambian de color de rojo a cualquier otro

5. **Confirmación de tendencia**:
   - Una secuencia de velas verdes confirma una tendencia alcista
   - Una secuencia de velas rojas confirma una tendencia a la baja
   - La alternancia de colores indica una tendencia lateral o incertidumbre

![indicator_elder_impulse_system](../../../../images/indicator_elder_impulse_system.png)

## Véase también

[EMA](ema.md)
[MACD](macd.md)
[MACDHistogram](macd_histogram.md)
[ForceIndex](force_index.md)