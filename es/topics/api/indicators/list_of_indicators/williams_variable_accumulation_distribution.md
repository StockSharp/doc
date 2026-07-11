# WVAD

**acumulación/distribución variable de Williams (WVAD)** es un indicador de volumen acumulativo desarrollado por Larry Williams. Evalúa la presión de compra y venta analizando la relación entre el precio de apertura, el precio de cierre, el máximo, el mínimo y el volumen de negociación.

Para utilizar el indicador, utilice la clase [WilliamsVariableAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsVariableAccumulationDistribution).

## Descripción

El indicador WVAD mide cuánto controlan los compradores o vendedores el movimiento de precios dentro de cada barra y pondera este valor por volumen. Si el precio de cierre es más alto que el precio de apertura, indica dominio del comprador y viceversa. El rango máximo-mínimo se utiliza como factor de normalización.

Principales aplicaciones del indicador:
- Confirmando la tendencia actual
- Identificar divergencias entre el indicador y el precio.
- Determinar la presión de compra o venta
- Evaluar la fuerza del movimiento de precios teniendo en cuenta el volumen

## Cálculo

El indicador WVAD se calcula mediante la siguiente fórmula:

```
WVAD = WVAD(previous) + ((Close - Open) / (High - Low)) * Volume
```

donde:
- Close - precio de cierre del período actual
- Open - precio de apertura del período actual
- High - precio más alto del período actual
- Low - precio más bajo del período actual
- Volume - volumen de operaciones del período actual
- WVAD(anterior) - valor del indicador anterior

Si High = Low (el rango es cero), el valor para ese período no se suma.

El indicador es acumulativo: los valores se acumulan con cada nuevo período.

## Véase también

[WAD](williams_accumulation_distribution.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
