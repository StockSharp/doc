# KST

El **indicador de certeza (KST)** es un indicador técnico desarrollado por Martin Pring que representa la suma de cuatro tasas de cambio suavizadas (ROC) con diferentes períodos para identificar ciclos de mercado a largo plazo.

Para utilizar el indicador, debe utilizar la clase [KnowSureThing](xref:StockSharp.Algo.Indicators.KnowSureThing).

## Descripción

El indicador de certeza (KST) es un oscilador desarrollado por Martin Pring para identificar tendencias midiendo el impulso de los precios en varios períodos de tiempo. El indicador combina cuatro mediciones de tasa de cambio (ROC) con diferentes períodos, dando más importancia a períodos más largos.

KST se basa en la teoría de que los ciclos de mercado de diferentes duraciones influyen colectivamente en el movimiento de precios. Al combinar ROC de diferentes períodos, KST tiene como objetivo identificar tendencias cíclicas a largo plazo y determinar posibles puntos de reversión.

El indicador suele ir acompañado de una línea de señal (media móvil de KST) y sus cruces se pueden utilizar para generar señales de negociación.

## Cálculo

El cálculo del indicador KST implica los siguientes pasos:

1. Calcule cuatro mediciones de tasa de cambio (ROC) con diferentes períodos:
   ```
   ROC1 = ((Close / Close[n1 periods ago]) - 1) * 100
   ROC2 = ((Close / Close[n2 periods ago]) - 1) * 100
   ROC3 = ((Close / Close[n3 periods ago]) - 1) * 100
   ROC4 = ((Close / Close[n4 periods ago]) - 1) * 100
   ```

2. Suavice cada ROC usando una media móvil simple (SMA):
   ```
   RCMA1 = SMA(ROC1, m1)
   RCMA2 = SMA(ROC2, m2)
   RCMA3 = SMA(ROC3, m3)
   RCMA4 = SMA(ROC4, m4)
   ```

3. Suma ponderada para obtener KST:
   ```
   KST = (RCMA1 * 1) + (RCMA2 * 2) + (RCMA3 * 3) + (RCMA4 * 4)
   ```

4. Calcular la línea de señal:
   ```
   Línea de señal = SMA(KST, período de señal)
   ```

donde:
- Close - precio de cierre
- n1, n2, n3, n4: períodos para el cálculo de ROC (valores predeterminados: 10, 15, 20, 30)
- m1, m2, m3, m4: períodos para el suavizado ROC (valores predeterminados: 10, 10, 10, 15)
- período de señal - período de línea de señal (valor predeterminado: 9)

## Interpretación

El indicador KST se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - Cuando KST cruza la línea cero de abajo hacia arriba, puede verse como una señal alcista.
   - Cuando KST cruza la línea cero de arriba a abajo, puede verse como una señal bajista.

2. **Cruces de línea de señal**:
   - Cuando KST cruza la línea de señal de abajo hacia arriba, puede verse como una señal alcista (más sensible que el cruce de la línea cero)
   - Cuando KST cruza la línea de señal de arriba a abajo, puede verse como una señal bajista (más sensible que el cruce de la línea cero)

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que KST forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que KST forma un máximo más bajo

4. **Valores extremos**:
   - High valores positivos KST pueden indicar condiciones de sobrecompra en el mercado
   - High valores negativos KST pueden indicar condiciones de sobreventa del mercado

5. **Dirección del movimiento**:
   - La tendencia ascendente KST indica un sentimiento general alcista del mercado
   - La caída de la tendencia KST indica un sentimiento general bajista del mercado

6. **Confirmación de tendencia**:
   - KST se puede utilizar para confirmar señales de otros indicadores
   - La coherencia entre la dirección KST y el precio confirma la fuerza de la tendencia actual

7. **Análisis del sentimiento del mercado**:
   - Los valores positivos de KST indican predominio del sentimiento alcista
   - Los valores negativos de KST indican predominio del sentimiento bajista

![Gráfico del indicador KST](../../../../images/indicator_kst.png)

## Véase también

[RoC](roc.md)
[MACD](macd.md)
[Impulso](momentum.md)
[RSI](rsi.md)
