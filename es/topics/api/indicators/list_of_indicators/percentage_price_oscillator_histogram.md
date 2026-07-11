# Histograma del PPO

El **Histograma del PPO (PPOH)** muestra la distancia entre la línea PPO y su línea de señal como un histograma, lo que ayuda a los operadores a evaluar inmediatamente el equilibrio del impulso.

Para utilizar el indicador, emplee la clase [PercentagePriceOscillatorHistogram](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorHistogram).

## Descripción

El histograma PPO se deriva del PPO estándar. En lugar de trazar tanto la línea PPO como la línea de señal, visualiza su diferencia como barras alrededor del nivel cero. Las barras positivas indican que la línea PPO está por encima de la línea de señal (impulso alcista), mientras que las barras negativas muestran que la línea PPO está por debajo de la línea de señal (impulso bajista).

El histograma reacciona rápidamente a los cambios en el diferencial entre el PPO y la línea de señal, lo que lo hace adecuado para detectar cambios tempranos en la fuerza de la tendencia o identificar divergencias de impulso.

## Cálculo

1. Calcule la línea PPO y su línea de señal usando los períodos deseados.
2. Reste la línea de señal de la línea PPO para obtener el valor del histograma.

```
Histogram = PPO - señal
```

Los valores superiores a cero resaltan la presión alcista, mientras que los inferiores a cero reflejan una presión bajista. La velocidad a la que las barras del histograma se expanden o contraen proporciona pistas sobre la aceleración o desaceleración del impulso.

## Interpretación

- **Cruces de la línea cero.** Moverse por encima de cero confirma que la línea PPO ha cruzado por encima de la línea de señal, lo que sugiere un cambio alcista. Caer por debajo de cero indica un cruce bajista.
- **Impulsos de momentum.** El rápido crecimiento de las barras positivas sugiere un fortalecimiento del impulso alcista; Las barras que se contraen insinúan un debilitamiento de la fuerza y ​​una posible reversión.
- **Divergencias.** Divergencia entre la acción del precio y el histograma puede alertar a los operadores sobre el posible agotamiento de la tendencia antes de que se vuelva visible en los gráficos de precios.

![indicator_percentage_price_oscillator_histogram](../../../../images/indicator_percentage_price_oscillator_histogram.png)

## Véase también

- [PPO](percentage_price_oscillator.md)
- [Señal del PPO](percentage_price_oscillator_signal.md)
