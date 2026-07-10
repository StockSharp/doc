# Bull Power

**Bull Power** es la contraparte alcista dentro del sistema Elder-ray. Mide con qué fuerza los compradores empujan los precios por encima de un
media móvil exponencial (EMA) comparando el máximo de la barra con el precio medio.

Utilice la clase [BullPower](xref:StockSharp.Algo.Indicators.BullPower) para trabajar con este indicador.

## Descripción

El indicador utiliza la fórmula:

`Bull Power = High − EMA`.

- Los valores positivos confirman la presión alcista y respaldan una tendencia alcista.
- Los valores que caen hacia cero o por debajo de cero indican un debilitamiento de los alcistas.
- Los picos extremos pueden preceder a las correcciones, especialmente cuando el EMA apunta hacia abajo.

## Parámetros

Bull Power hereda sus parámetros de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — Período EMA.
- **Alpha** (opcional): coeficiente de suavizado, cuando corresponda.

## Uso

- El aumento de Bull Power junto con un aumento de EMA confirma la fuerza de la tendencia.
- El precio alcanza nuevos máximos sin lecturas más altas de Bull Power forma una divergencia bajista.
- Combine Bull y Bear Power con el precio EMA para evaluar la estructura completa de [Elder Ray](elder_ray.md).

![indicator_bull_power](../../../../images/indicator_bull_power.png)

## Véase también

[Bear Power](bear_power.md)
[Elder Ray](elder_ray.md)
[Media móvil exponencial](ema.md)
