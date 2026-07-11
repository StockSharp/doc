# ADL

**línea de acumulación/distribución (ADL)** es un indicador de volumen desarrollado por Mark Chaikin. El indicador evalúa la relación de oferta y demanda en el mercado analizando la correlación entre precio y volumen.

Para utilizar el indicador, debe utilizar la clase [AccumulationDistributionLine](xref:StockSharp.Algo.Indicators.AccumulationDistributionLine).

## Descripción

La línea Accumulation/Distribution es un indicador acumulativo que utiliza el volumen y el precio para determinar si un valor se encuentra en una fase de acumulación (compra) o de distribución (venta).

El indicador ADL ayuda a confirmar una tendencia o advertir sobre su posible reversión:
- Si el precio sube y ADL baja, esto puede indicar debilidad en una tendencia alcista.
- Si el precio cae y ADL aumenta, esto puede indicar una posible reversión de una tendencia a la baja.

## Cálculo

El cálculo de la línea Accumulation/Distribution se produce en dos pasos:

**1. Cálculo del multiplicador de volumen (CLV - Valor de ubicación cercana):**
```
CLV = ((Close - Low) - (High - Close)) / (High - Low)
```

**2. Cálculo de ADL:**
```
ADL = valor ADL anterior + CLV * Volume
```

donde:
- Close - precio de cierre del período
- Low - precio mínimo del período
- High - precio máximo del período
- Volume - volumen de operaciones para el período

Si (High - Low) es igual a cero, CLV se establece en cero.

![IndicatorAccumulationDistributionLine](../../../../images/indicator_accumulation_distribution_line.png)

## Véase también

[OBV](on_balance_volume.md)
