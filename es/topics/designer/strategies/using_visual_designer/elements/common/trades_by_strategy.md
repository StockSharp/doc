# Operaciones por estrategia

![Designer The transaction strategy 00](../../../../../../images/designer_trades_strategy_00.png)

El cubo se usa para obtener todas las operaciones de la estrategia. 

## Sockets de entrada

- **Instrument** - instrumento para el que necesita obtener operaciones. Si el instrumento no se pasa, se transfieren a la salida las operaciones de todos los instrumentos de la estrategia.

## Sockets de salida

- **Trades** - operaciones que surgen del instrumento pasado. Pueden usarse tanto para mostrarlas en el gráfico con el elemento **Chart panel**, como para protección de posición mediante el elemento **Position protection**.

