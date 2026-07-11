# BV

**volumen de balance (BV)** es un indicador técnico que rastrea la acumulación y distribución del volumen de operaciones en función de los cambios de precios.

Para utilizar el indicador, debe utilizar la clase [BalanceVolume](xref:StockSharp.Algo.Indicators.BalanceVolume).

## Descripción

El indicador volumen de balance (BV) está diseñado para analizar la relación entre el cambio de precio y el volumen de operaciones. Ayuda a los operadores a determinar cómo los cambios de volumen se corresponden con el movimiento de precios, lo que puede indicar la fortaleza o debilidad de la tendencia actual.

La idea principal de BV es que el volumen debería confirmar la dirección del precio. Si el precio aumenta al aumentar el volumen, esto indica una fuerte tendencia alcista. Por el contrario, si el precio cae al aumentar el volumen, esto sugiere una fuerte tendencia a la baja.

El indicador BV es particularmente útil para:
- Confirmando la fuerza de la tendencia actual
- Identificar posibles cambios de tendencia
- Detectar divergencias entre precio y volumen
- Determinación de los niveles de acumulación y distribución.

## Cálculo

El cálculo del indicador volumen de balance se basa en comparar el precio de cierre con el precio de cierre anterior y ponderar el volumen de negociación:

```
Si Close > cierre anterior:
	BV = Previous BV + Volume
Si Close < cierre anterior:
	BV = Previous BV - Volume
Si Close = cierre anterior:
	BV = Previous BV
```

donde:
- Close - precio de cierre actual
- Anterior Close - precio de cierre anterior
- Volume - volumen de operaciones actual
- BV anterior - valor anterior del indicador volumen de balance

## Interpretación

- **BV aumenta con el aumento de precio** - confirmación de una tendencia alcista, indica un fuerte interés de los compradores
- **BV cayendo con disminución de precio** - confirmación de una tendencia a la baja, indica un fuerte interés del vendedor
- **BV sube con precio estable o baja** - acumulación potencial, puede preceder a una reversión al alza
- **BV cayendo con precio estable o en aumento** - distribución potencial, puede preceder a una reversión a la baja
- **Divergencia entre BV y precio** - advertencia de una posible inversión de tendencia:
  - Si el precio sube mientras BV cae, una rápida reversión a la baja puede ser inminente
  - Si el precio cae mientras BV sube, una rápida reversión al alza puede ser inminente

![indicator_balance_volume](../../../../images/indicator_balance_volume.png)

## Véase también

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)