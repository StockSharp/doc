# Momentum

El indicador **Momentum** mide la magnitud del cambio de precio de un instrumento financiero durante un período determinado. Indica momentos de sobrecompra y sobreventa en los que la curva alcanza valores máximos o mínimos. Agregar una media móvil suavizada al indicador mejora la interpretación de los cambios de tendencia.

Para utilizar el indicador, se debe utilizar la clase [Momentum](xref:StockSharp.Algo.Indicators.Momentum).
##### Cálculo

Momentum se define como la relación entre el precio de hoy y el precio de hace n períodos:

MOMENTO = CIERRE(i) / CIERRE(i - n) * 100

donde:
CLOSE(i) — el precio de cierre de la barra actual;
CLOSE(i - n) — el precio de cierre de n barras atrás.


![IndicatorMomentum](../../../../images/indicatormomentum.png)

## Véase también

[Índice de flujo de dinero](money_flow_index.md)
