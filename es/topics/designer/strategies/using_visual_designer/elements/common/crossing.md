# Cruce

![Designer Crossing 00](../../../../../../images/designer_crossing_00.png)

Este elemento se usa para seguir la posición de dos valores entre sí. Por ejemplo, para determinar el momento de cruce entre dos líneas.

La comparación se realiza con respecto a los valores en dos sockets **Up** y **Down**.

## Sockets de entrada

- **Up** – valores que permiten comparación (por ejemplo, valor numérico, valor de indicador, etc.).
- **Down** – valores que permiten comparación (por ejemplo, valor numérico, valor de indicador, etc.).

## Sockets de salida

- **Flag** – true si **Up** es mayor que **Down**, de lo contrario false.

![Designer Crossing 01](../../../../../../images/designer_crossing_01.png)

Ejemplo de uso del bloque Crossing para seguir los cruces de dos [indicadores SMA](../../../../../api/indicators/list_of_indicators/sma.md). Se usan dos bloques Crossing, y cada uno genera true por separado según cuándo la SMA larga sea mayor que la corta y cuándo sea menor.

## Véase también

[Retraso de valor](delay_value.md)
