# OBV

**volumen en balance (OBV)** es un indicador técnico desarrollado por Joseph Granville que utiliza el volumen de operaciones para pronosticar cambios de precios acumulando volumen según la dirección del precio.

Para utilizar el indicador, debe utilizar la clase [OnBalanceVolume](xref:StockSharp.Algo.Indicators.OnBalanceVolume).

## Descripción

volumen en balance (OBV) es un indicador acumulativo que agrega volumen cuando el precio de cierre sube y resta volumen cuando el precio de cierre baja. El indicador se basa en el concepto de que los cambios de volumen preceden a los cambios de precios. Según esta teoría, cuando el volumen aumenta significativamente sin un cambio correspondiente en el precio, se debe esperar que el precio eventualmente aumente, y viceversa.

OBV tiene como objetivo detectar momentos en los que el "dinero inteligente" (grandes inversores institucionales) está acumulando o distribuyendo posiciones, lo que puede predecir futuros movimientos de precios. El indicador es particularmente útil para identificar divergencias entre precio y volumen que pueden indicar posibles reversiones del mercado.

El indicador OBV fue introducido por primera vez por Joseph Granville en 1963 en su libro "La nueva clave de Granville para los beneficios del mercado de valores" y desde entonces se ha convertido en uno de los indicadores de volumen más utilizados.

## Cálculo

El cálculo del Volume en equilibrio es muy sencillo:

1. Establezca el valor inicial OBV (normalmente 0 o un número arbitrario):
   ```
   OBV[initial] = 0
   ```

2. Para cada período posterior:
   ```
   Si Close[current] > Close[previous], entonces:
       OBV[current] = OBV[previous] + Volume[current]
   Si Close[current] < Close[previous], entonces:
       OBV[current] = OBV[previous] - Volume[current]
   Si Close[current] = Close[previous], entonces:
       OBV[current] = OBV[previous]
   ```

donde:
- Close - precio de cierre
- Volume - volumen de operaciones

## Interpretación

volumen en balance se puede interpretar de la siguiente manera:

1. **Análisis de tendencias**:
   - El aumento de OBV indica volumen que ingresa al mercado (acumulación), lo que puede predecir un aumento de precios
   - La caída de OBV indica que el volumen sale del mercado (distribución), lo que puede predecir una caída de precios.
   - Flat OBV indica que no hay movimiento de volumen direccional, lo que puede corresponder a una tendencia lateral

2. **Confirmación de tendencia de precios**:
   - Si OBV se mueve en la misma dirección que el precio, esto confirma la tendencia actual del precio.
   - Si OBV y el precio se mueven en direcciones opuestas, esto puede indicar un posible cambio de tendencia.

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que OBV forma un mínimo más alto (señal de compra)
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que OBV forma un máximo más bajo (señal de venta)

4. **Rupturas del OBV**:
   - La ruptura del OBV de un nivel de resistencia o soporte a menudo precede a una ruptura similar en el gráfico de precios
   - Los operadores pueden utilizar las rupturas de la línea de tendencia OBV para pronosticar movimientos futuros de precios

5. **Línea base**:
   - Algunos operadores utilizan las medias móviles OBV como líneas "de referencia"
   - El cruce del OBV con su media móvil puede generar señales de negociación

6. **Análisis técnico Patterns**:
   - En el gráfico OBV se pueden formar patrones de análisis técnico clásicos, como "cabeza y hombros", "doble fondo", etc.
   - Estos patrones pueden proporcionar señales de trading adicionales.

7. **XQX000Picos XQX**:
   - Los cambios bruscos y repentinos en OBV pueden indicar cambios significativos en el sentimiento del mercado
   - Estos "picos de volumen" suelen preceder a movimientos sustanciales de precios.

Es importante tener en cuenta que OBV es un indicador acumulativo, por lo que su valor absoluto no es de gran importancia. Lo que importa es la dirección del movimiento OBV y su relación con el movimiento del precio.

![indicator_on_balance_volume](../../../../images/indicator_on_balance_volume.png)

## Véase también

[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)
[NegativeVolumeIndex](negative_volume_index.md)
