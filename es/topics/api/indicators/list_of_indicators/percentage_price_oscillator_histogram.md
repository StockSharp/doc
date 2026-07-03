# Percentage Price Oscillator Histogram

El **Percentage Price Oscillator Histogram (PPOH)** muestra la distancia entre el PPO line y su línea de señal como un histograma, lo que ayuda a los operadores a evaluar inmediatamente el equilibrio del impulso.

Para utilizar el indicador, emplee la clase [PercentagePriceOscillatorHistogram](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorHistogram).

## Descripción

El histograma PPO se deriva del estándar Percentage Price Oscillator (PPO). En lugar de trazar tanto el PPO como las líneas de señal, visualiza su diferencia como barras alrededor del nivel cero. Las barras positivas indican que PPO line está por encima de la línea de señal (impulso alcista), mientras que las barras negativas muestran que PPO line está por debajo de la línea de señal (impulso bajista).

El histograma reacciona rápidamente a los cambios en el diferencial entre el PPO y la línea de señal, lo que lo hace adecuado para detectar cambios tempranos en la fuerza de la tendencia o identificar divergencias de impulso.

## Cálculo

1. Calcule el PPO line y su línea de señal usando los períodos deseados.
2. Reste la línea de señal del PPO line para obtener el valor del histograma.

```
Histogram = PPO - Signal
```

Los valores superiores a cero resaltan la presión alcista, mientras que los inferiores a cero reflejan una presión bajista. La velocidad a la que las barras del histograma se expanden o contraen proporciona pistas sobre la aceleración o desaceleración del impulso.

## Interpretación

- **Zero line crossovers.** Moverse por encima de cero confirma que el PPO line ha cruzado por encima de la línea de señal, lo que sugiere un cambio alcista. Caer por debajo de cero indica un cruce bajista.
- **Momentum surges.** El rápido crecimiento de las barras positivas sugiere un fortalecimiento del impulso alcista; Las barras que se contraen insinúan un debilitamiento de la fuerza y ​​una posible reversión.
- **Divergencias.** Divergencia entre la acción del precio y el histograma puede alertar a los operadores sobre el posible agotamiento de la tendencia antes de que se vuelva visible en los gráficos de precios.

![indicator_percentage_price_oscillator_histogram](../../../../images/indicator_percentage_price_oscillator_histogram.png)

## Véase también

- [Percentage Price Oscillator](percentage_price_oscillator.md)
- [Percentage Price Oscillator Signal](percentage_price_oscillator_signal.md)
