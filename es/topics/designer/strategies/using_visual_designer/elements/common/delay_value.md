# Retraso de valor

![Designer Delay 00](../../../../../../images/designer_delay_00.png)

Este componente se usa para retrasar la transmisión de un valor durante un número especificado de iteraciones.

## Sockets de entrada

- **Trigger** – señal (cualquier valor excepto `False`) que inicializa el contador interno para iniciar la cuenta atrás del retraso.
- **Input** - cualquier valor entrante (excepto [velas no finalizadas](../data_sources/candles.md) o [valores de indicador no finales](indicator.md)) que disminuye el contador interno. Cuando el contador llega a cero, se desactiva y el socket de salida se activa. Si el contador no fue activado por **Trigger**, los valores entrantes se ignoran.

## Sockets de salida

- **Signal** – emite una señal cuando el contador llega a cero, indicando el final del retraso.

## Parámetros

- **Duration** - especifica la duración del retraso en iteraciones.

![Designer Delay 01](../../../../../../images/designer_delay_01.png)

## Véase también

- [Comparación](comparison.md)
