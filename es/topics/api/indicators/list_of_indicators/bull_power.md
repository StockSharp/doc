# fuerza alcista

**fuerza alcista** es la contraparte alcista dentro del sistema de rayos de Elder. Mide con qué fuerza los compradores empujan los precios por encima de una
media móvil exponencial (EMA) comparando el máximo de la barra con el precio medio.

Utilice la clase [BullPower](xref:StockSharp.Algo.Indicators.BullPower) para trabajar con este indicador.

## Descripción

El indicador utiliza la fórmula:

`fuerza alcista = High − EMA`.

- Los valores positivos confirman la presión alcista y respaldan una tendencia alcista.
- Los valores que caen hacia cero o por debajo de cero indican un debilitamiento de los alcistas.
- Los picos extremos pueden preceder a las correcciones, especialmente cuando el EMA apunta hacia abajo.

## Parámetros

fuerza alcista hereda sus parámetros de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Longitud** — Período EMA.
- **Alfa** (opcional): coeficiente de suavizado, cuando corresponda.

## Uso

- El aumento de fuerza alcista junto con un aumento de EMA confirma la fuerza de la tendencia.
- El precio alcanza nuevos máximos sin lecturas más altas de fuerza alcista forma una divergencia bajista.
- Combine fuerza alcista y fuerza bajista con el precio EMA para evaluar la estructura completa de [rayos de Elder](elder_ray.md).

![Gráfico del indicador fuerza alcista](../../../../images/indicator_bull_power.png)

## Véase también

[fuerza bajista](bear_power.md)
[rayos de Elder](elder_ray.md)
[Media móvil exponencial](ema.md)
