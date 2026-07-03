# Bear Power

**Bear Power** es parte del sistema Elder-ray de Alexander Elder y muestra cuán fuertes son los vendedores en comparación con un exponencial
media móvil (EMA). Mide hasta qué punto los mínimos intradiarios caen por debajo del precio promedio y destaca los momentos en los que los bajistas pierden.
controlar.

Utilice la clase [BearPower](xref:StockSharp.Algo.Indicators.BearPower) para acceder al indicador.

## Descripción

El indicador se calcula como la diferencia entre el mínimo de la barra y el valor EMA:

`Bear Power = Low − EMA`.

- Las lecturas negativas confirman la presión vendedora.
- Los valores crecientes hacia cero o por encima de cero indican un debilitamiento de los bajistas y una posible reversión alcista.
- Las caídas profundas a menudo preceden a los rebotes, especialmente durante las ventas masivas provocadas por el pánico.

## Parámetros

Bear Power hereda la configuración de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — Período EMA.
- **Alpha** (opcional): coeficiente de suavizado si el EMA está configurado de esta manera.

## Uso

- Busque reversiones cuando Bear Power suba después de un mínimo extremo mientras que EMA comienza a subir.
- El cruce de la línea cero puede confirmar un cambio en la tendencia predominante.
- Combine Bear Power con [Bull Power](bull_power.md) y el precio EMA para crear el indicador [Elder Ray](elder_ray.md) completo.

![indicator_bear_power](../../../../images/indicator_bear_power.png)

## Véase también

[Bull Power](bull_power.md)
[Elder Ray](elder_ray.md)
[ExponentialMovingAverage](ema.md)
