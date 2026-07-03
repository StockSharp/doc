# RC

**Rainbow Charts (RC)** es un indicador de análisis técnico que consta de un conjunto de medias móviles con diferentes períodos mostrados en un único gráfico. Visualmente, el indicador se parece a un arco iris, de ahí su nombre.

Para utilizar el indicador, debe utilizar la clase [RainbowCharts](xref:StockSharp.Algo.Indicators.RainbowCharts).

## Descripción

Rainbow Charts se basan en el uso de múltiples medias móviles (normalmente SMA simple) con períodos progresivamente crecientes. Las diferentes líneas de media móvil están coloreadas en diferentes colores, creando un efecto de arco iris en el gráfico.

El indicador ayuda a determinar la dirección y la fuerza de la tendencia:
- Cuando las líneas divergen, esto indica un fortalecimiento de la tendencia.
- Cuando las líneas convergen, esto puede indicar un debilitamiento de la tendencia o una posible reversión.
- Cuando el precio está por encima de todas las líneas, indica una fuerte tendencia alcista.
- Cuando el precio está por debajo de todas las líneas, indica una fuerte tendencia a la baja.

## Parámetros

- **Lines**: número de medias móviles SMA utilizadas en el gráfico de arcoíris.

## Cálculo

Rainbow Charts consta de múltiples promedios móviles (SMA), y el período de cada línea posterior aumenta en un cierto paso. Para n líneas con un período base p, los períodos se calculan como:

```
Period(i) = p + i * step
```

donde:
- i - número de línea (de 0 a n-1)
- paso - paso de aumento del período (generalmente 1)

![IndicatorRainbowCharts](../../../../images/indicator_rainbow_charts.png)

## Véase también

[SMA](sma.md)