# Señal del PPO

El indicador **Señal del PPO (PPOS)** amplía el PPO estándar agregando la línea de señal asociada que generalmente se usa para filtrar operaciones.

Para utilizar el indicador, emplee la clase [PercentagePriceOscillatorSignal](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorSignal).

## Descripción

El oscilador porcentual de precio (PPO) mide la diferencia porcentual entre dos medias móviles exponenciales (EMA). La versión de señal se centra en suavizar la línea PPO con una EMA adicional, ayudando a los operadores a reaccionar sólo ante los cambios más persistentes en el impulso.

El indicador está formado por los siguientes componentes:

1. **Línea PPO**: la diferencia porcentual entre los EMA rápidos y lentos.
2. **Línea de señal** – un EMA calculado a partir de la línea PPO (9 períodos por defecto).

Cuando la línea PPO cruza por encima de la línea de señal, sugiere un impulso alcista creciente; cruzar por debajo indica un fortalecimiento del impulso bajista. Permanecer por encima o por debajo de la línea de señal puede confirmar la fuerza de la tendencia predominante.

## Cálculo

1. Calcule las EMA rápidas y lentas de la serie de precios seleccionada.
2. Calcule la línea PPO como la distancia porcentual entre los EMA rápidos y lentos.
3. Alise la línea PPO con un EMA para obtener la línea de señal.

```
FastEMA = EMA(Price, ShortPeriod)
SlowEMA = EMA(Price, LongPeriod)
PPO = ((FastEMA - SlowEMA) / SlowEMA) * 100
Signal = EMA(PPO, SignalPeriod)
```

## Interpretación

- **Cruces de señales.** Se produce una señal alcista cuando la línea PPO cruza la línea de señal desde abajo; el cruce opuesto apunta a un impulso bajista.
- **Confirmación de tendencia.** Mantenerse por encima de la línea de señal confirma una tendencia alcista, mientras que mantenerse por debajo respalda una tendencia bajista.
- **Divergencias.** Una divergencia entre la acción del precio y la línea PPO mientras interactúa con la línea de señal puede anticipar reversiones.

![Señal del PPO](../../../../images/indicator_percentage_price_oscillator_signal.png)

## Véase también

- [PPO](percentage_price_oscillator.md)
- [Histograma del PPO](percentage_price_oscillator_histogram.md)
