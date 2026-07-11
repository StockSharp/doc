# TWAP

**Precio medio ponderado por tiempo (TWAP)** es un indicador que calcula el precio promedio de un instrumento financiero ponderado por tiempo durante un período específico. TWAP es ampliamente utilizado por inversores institucionales para ejecutar órdenes grandes con un impacto mínimo en el mercado.

Para utilizar el indicador, debe utilizar la clase [TimeWeightedAveragePrice](xref:StockSharp.Algo.Indicators.TimeWeightedAveragePrice).

## Descripción

TWAP es uno de los algoritmos de ejecución de órdenes más comunes que divide una orden grande en una serie de órdenes más pequeñas distribuidas uniformemente a lo largo del tiempo. El objetivo de TWAP es obtener un precio promedio durante un intervalo de tiempo específico minimizando el impacto en el mercado.

Aplicaciones principales de TWAP:
- Precio de referencia para evaluar la calidad de ejecución de órdenes
- Algoritmo de ejecución para minimizar el impacto en el mercado.
- Herramienta para análisis de mercado y toma de decisiones de trading.

A diferencia de VWAP (precio medio ponderado por volumen), TWAP no considera los volúmenes de negociación, centrándose exclusivamente en el aspecto temporal.

## Cálculo

El cálculo de TWAP se realiza sumando los precios en intervalos de tiempo iguales y dividiendo esta suma por el número de intervalos de tiempo:

```
TWAP = (P₁ + P₂ + P₃ + ... + Pₙ) / n
```

donde:
- P₁, P₂, ..., Pₙ - precios en momentos sucesivos
- n - número de intervalos de tiempo

En la implementación práctica, los precios típicos para cada período (vela) se utilizan con mayor frecuencia:

```
Precio típico = (High + Low + Close) / 3
TWAP = Sum(precio típico) / número de periodos
```

También se puede utilizar una fórmula recursiva para determinar el valor TWAP actual en tiempo real:

```
TWAP(current) = (TWAP(previous) * (n-1) + P(current)) / n
```

donde n es el número de observaciones en la ventana TWAP.

![Gráfico del indicador TWAP](../../../../images/indicator_time_weighted_average_price.png)
