# EIS

**Sistema de impulso de Elder (EIS)** es un indicador técnico desarrollado por el Dr. Alexander Elder que combina un indicador de tendencia y un oscilador de impulso para determinar la dirección y la fuerza del movimiento del mercado.

Para utilizar el indicador, debe utilizar la clase [ElderImpulseSystem](xref:StockSharp.Algo.Indicators.ElderImpulseSystem).

## Descripción

El sistema de impulso de Elder (EIS) es una herramienta sencilla pero potente para visualizar el impulso del mercado. Combina dos indicadores:
1. **Media móvil exponencial (EMA)** - para determinar la dirección de la tendencia
2. **Histograma MACD** - para medir la fuerza y el impulso del movimiento de precios

EIS clasifica cada vela en el gráfico de precios en una de tres categorías (generalmente indicadas por diferentes colores):
- **Verde (fuerte impulso alcista)** - cuando ambos indicadores están aumentando
- **Rojo (fuerte impulso bajista)** - cuando ambos indicadores están cayendo
- **Azul o neutral (sin impulso claro)** - cuando los indicadores se mueven en direcciones opuestas

EIS es particularmente útil para:
- Determinación visual rápida de la dirección y la fuerza de la tendencia
- Identificar puntos de entrada y salida en la dirección de la tendencia principal.
- Identificar posibles puntos de reversión
- Filtrar señales falsas

## Cálculo

El cálculo del sistema de impulso de Elder implica los siguientes pasos:

1. Calcule la media móvil exponencial de 13 períodos (EMA):
   ```
   EMA = EMA(Close, 13)
   ```

2. Calcule el histograma MACD (valores estándar: 12, 26, 9):
   ```
   Línea MACD = EMA(Close, 12) - EMA(Close, 26)
   Línea de señal = EMA(Línea MACD, 9)
   Histograma MACD = Línea MACD - Línea de señal
   ```

3. Determine la clasificación de color de la vela actual:
   ```
   Si EMA[actual] > EMA[anterior] Y Histograma MACD[actual] > Histograma MACD[anterior], entonces Verde (impulso alcista)
   Si EMA[actual] < EMA[anterior] Y Histograma MACD[actual] < Histograma MACD[anterior], entonces Rojo (impulso bajista)
   En caso contrario Azul (sin impulso)
   ```

## Interpretación

El sistema de impulso de Elder se interpreta de la siguiente manera:

1. **Velas verdes (fuerte impulso alcista)**:
   - Indicar un fuerte impulso alcista
   - El mejor momento para comprar o mantener posiciones largas
   - Una serie de velas verdes indica una fuerte tendencia alcista

2. **Velas rojas (fuerte impulso bajista)**:
   - Indica un fuerte impulso a la baja
   - Mejor momento para vender o mantener posiciones cortas
   - Una serie de velas rojas indica una fuerte tendencia a la baja.

3. **Velas azules (sin impulso claro)**:
   - Indicar incertidumbre o consolidación.
   - Señalan una posible desaceleración o cambio de tendencia
   - A menudo aparecen durante períodos de consolidación o antes de un cambio de tendencia.

4. **Estrategias de trading**:
   - Compre cuando las velas cambien de color de azul a verde
   - Vender cuando las velas cambien de color de azul a rojo.
   - Cierre posiciones largas cuando las velas cambien de color de verde a cualquier otro
   - Cierre posiciones cortas cuando las velas cambien de color de rojo a cualquier otro

5. **Confirmación de tendencia**:
   - Una secuencia de velas verdes confirma una tendencia alcista
   - Una secuencia de velas rojas confirma una tendencia a la baja
   - La alternancia de colores indica una tendencia lateral o incertidumbre

![indicator_elder_impulse_system](../../../../images/indicator_elder_impulse_system.png)

## Véase también

[EMA](ema.md)
[MACD](macd.md)
[Histograma MACD](macd_histogram.md)
[FI](force_index.md)
