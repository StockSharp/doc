# Shift

**Shift** es un indicador auxiliar que compensa el flujo de valor entrante en un número específico de períodos. No transforma el
datos; solo lo retrasa o alinea para su uso en cálculos compuestos.

Utilice la clase [Shift](xref:StockSharp.Algo.Indicators.Shift) para acceder al indicador.

## Descripción

El indicador mantiene un búfer de los últimos valores y genera el que llegó hace **Length** barras. Si el historial de datos es
más corto que el desplazamiento requerido, el valor se considera indefinido.

## Parámetros

- **Length**: número de períodos en los que se desplazan los datos.

## Uso

- Alinear señales de diferentes indicadores en el tiempo.
- Cree indicadores y estrategias personalizados que requieran aportes retrasados.
- Cree series sintéticas, por ejemplo, para calcular la diferencia entre los precios actuales y los valores pasados.

![indicator_shift](../../../../images/indicator_shift.png)

## Véase también

[PassThrough](pass_through.md)
[Sum](sum_n.md)
