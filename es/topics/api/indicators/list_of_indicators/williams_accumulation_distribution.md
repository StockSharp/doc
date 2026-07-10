# WAD

**Williams Accumulation/Distribution (WAD)** es un indicador de volumen desarrollado por Larry Williams. A diferencia de la línea tradicional Accumulation/Distribution, el indicador WAD se centra en la relación entre el precio de cierre del período actual y el precio de cierre del período anterior para determinar la presión del comprador o del vendedor.

Para utilizar el indicador, debe utilizar la clase [WilliamsAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsAccumulationDistribution).

## Descripción

El indicador Williams Accumulation/Distribution está diseñado para identificar discrepancias entre el precio y el volumen que pueden indicar posibles cambios de tendencia. WAD es particularmente útil para revelar debilidades en el movimiento actual de precios.

Características clave de WAD:
- Los valores positivos indican acumulación (presión de compra)
- Los valores negativos indican distribución (presión de venta)
- Divergencias entre WAD y el precio pueden preceder a las reversiones de precios

Principales aplicaciones del indicador:
- Confirmando la tendencia actual
- Identificar posibles reversiones de precios
- Determinar la presión del comprador o vendedor

## Cálculo

El indicador Williams Accumulation/Distribution se calcula utilizando la siguiente lógica:

1. Determine la protección del rango verdadero (TRP) para el período actual:
   ```
   TRP = Max(High - Low, |High - Close_prev|, |Low - Close_prev|)
   ```

2. Calcule el valor Accumulation/Distribution (AD) para el período actual:
   - Si Close > Close_prev (comercializar hacia arriba):
      ```
      AD = Close - Min(Low, Close_prev) 
      ```
   - Si Close < Close_prev (mercado a la baja):
      ```
      AD = Close - Max(High, Close_prev)
      ```
   - Si Close = Cerrar_anterior:
      ```
      AD = 0
      ```

3. Calcule el valor WAD acumulando valores AD:
   ```
   WAD = Previous WAD value + AD
   ```

El indicador acumula valores positivos y negativos, formando una línea acumulativa que puede usarse para comparar con el movimiento de precios.

![IndicatorWilliamsAccumulationDistribution](../../../../images/indicator_williams_accumulation_distribution.png)

## Véase también

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
