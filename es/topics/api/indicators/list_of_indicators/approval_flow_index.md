# AFI

**Approval Flow Index (AFI)** es un indicador que mide la fuerza de la tendencia en función de la relación entre el volumen y el movimiento del precio.

Para utilizar el indicador, debe utilizar la clase [ApprovalFlowIndex](xref:StockSharp.Algo.Indicators.ApprovalFlowIndex).

## Descripción

El Approval Flow Index (AFI) ayuda a evaluar la intensidad del flujo de pedidos en el mercado y determinar la fuerza de la tendencia actual. Este indicador analiza la relación entre el volumen de operaciones y el movimiento de precios para identificar posibles puntos de reversión o confirmar la continuación de la tendencia.

El indicador AFI se puede utilizar para:
- Determinando la fuerza de la tendencia actual
- Identificar divergencias entre el precio y el indicador.
- Buscando posibles puntos de reversión del mercado

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo del indicador

## Cálculo

El cálculo de Approval Flow Index se basa en el análisis de la variación de precios y el volumen durante un período específico:

1. Primero, calcule el cambio de precio para el período.
2. Luego relacione este cambio con el volumen de operaciones.
3. Los valores resultantes se suman durante el período seleccionado (parámetro Length)

AFI tiene como objetivo determinar cuánto volumen de operaciones "aprueba" el movimiento del precio.

Los valores positivos de AFI indican una fuerte tendencia al alza, mientras que los valores negativos sugieren una tendencia a la baja. Los valores cercanos a cero pueden indicar la ausencia de una tendencia pronunciada.

![indicator_approval_flow_index](../../../../images/indicator_approval_flow_index.png)

## Véase también

[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)