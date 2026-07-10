# MCO

**McClellan Oscillator (MCO)** es un indicador técnico desarrollado por Sherman y Marian McClellan que mide la amplitud del mercado analizando la diferencia entre los promedios móviles de las acciones que suben y bajan.

aara utilizar el indicador, debe utilizar la clase [McClellanOscillator](xref:StockSharp.Algo.Indicators.McClellanOscillator).

## Descripción

El oscilador McClellan (MCO) es uno de los indicadores de amplitud del mercado más conocidos que ayuda a evaluar la condición general del mercado e identificar posibles puntos de reversión. Desarrollado en 1969, desde entonces se ha convertido en una herramienta crucial para muchos analistas técnicos.

MCO se basa en el análisis de la relación entre el número de acciones en alza y en baja en el mercado. El indicador calcula la diferencia entre las medias móviles exponenciales de avances netos de 19 y 39 períodos (diferencia entre el número de acciones que suben y bajan).

El oscilador McClellan es particularmente útil para:
- Determinar la dirección general del mercado
- Identificar condiciones de sobrecompra y sobreventa
- Identificar posibles puntos de reversión
- Confirmar la fortaleza o debilidad de la tendencia actual

## Cálculo

El cálculo del oscilador McClellan implica los siguientes pasos:

1. Calcule los Avances Netos para cada día de negociación:
   ```
   Net Advances = Advances - Declines
   ```
   donde Avances es el número de acciones en alza, Declives es el número de acciones en caída.

2. Calcule el promedio móvil exponencial de 19 períodos de los avances netos:
   ```
   EMA19 = EMA(Net Advances, 19)
   ```

3. Calcule el promedio móvil exponencial de 39 períodos de avances netos:
   ```
   EMA39 = EMA(Net Advances, 39)
   ```

4. Calcule el oscilador McClellan como la diferencia entre estos dos EMA:
   ```
   MCO = EMA19 - EMA39
   ```

## Interpretación

El oscilador McClellan se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - MCO cruzar la línea cero de abajo hacia arriba puede verse como una señal alcista, lo que indica un posible inicio de tendencia alcista.
   - MCO cruzar la línea cero de arriba a abajo puede verse como una señal bajista, lo que indica un posible inicio de tendencia bajista.

2. **Valores extremos**:
   - Los valores superiores a +100 suelen indicar condiciones de sobrecompra en el mercado.
   - Los valores por debajo de -100 a menudo indican condiciones de sobreventa del mercado.
   - Valores extremos (+150/-150 y por encima/por debajo) pueden indicar una posible reversión del mercado

3. **Divergencias**:
   - Divergencia alcista: el índice forma un nuevo mínimo, mientras que MCO forma un mínimo más alto
   - Divergencia bajista: el índice forma un nuevo máximo, mientras que MCO forma un máximo más bajo

4. **Estado de amplitud del mercado**:
   - Los valores positivos de MCO indican que la mayoría de las acciones del mercado están subiendo
   - Los valores negativos de MCO indican que la mayoría de las acciones del mercado están cayendo

5. **Movimiento Aceleración/Desaceleración**:
   - Los valores crecientes de MCO (positivos o negativos) indican una aceleración del movimiento actual del mercado.
   - Los valores decrecientes de MCO indican una desaceleración del movimiento actual del mercado

6. **Combinando con el índice de suma McClellan**:
   - El índice de suma McClellan (MSI) es la suma acumulativa de los valores MCO
   - MSI cruzando cero puede confirmar señales MCO e indicar cambios de tendencia a largo plazo

7. **Patrones alcistas/bajistas**:
   - "Cola alcista": rápida caída de MCO seguida de una rápida recuperación, que a menudo indica un posible fondo del mercado.
   - "Cola bajista": rápido aumento de MCO seguido de una rápida caída, que a menudo indica un máximo potencial del mercado

![indicator_mcclellan_oscillator](../../../../images/indicator_mcclellan_oscillator.png)

## Véase también

[HighLowIndex](high_low_index.md)
