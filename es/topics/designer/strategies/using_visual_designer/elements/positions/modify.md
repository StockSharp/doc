# Modificar posición

![Designer position modify 00](../../../../../../images/designer_position_modify_00.png)

El componente "Modify Position" se usa para cambiar una posición de trading según condiciones especificadas.

## Sockets de entrada

- **Security**: instrumento para el que se modificará la posición.
- **Trigger**: señal para activar la modificación de la posición.
- **Portfolio**: cartera dentro de la cual ocurre la operación.
- **Volume** (opcional): volumen para operaciones "Increase" y "Decrease". No se usa para "Reverse" y "Close Position".
- **Last Price** y **Last Volume**: para los algoritmos "VWAP" e "Iceberg" se requieren datos sobre el último precio y volumen de la transacción.
- **Cancel**: señal para cancelar la configuración de posición, por ejemplo, por timeout.

## Sockets de salida

- **Order**: información sobre la orden colocada.
- **Transaction**: información sobre la transacción realizada sobre la orden.
- **Balance**: este socket transmite información sobre la parte de la posición que no se realizó al final de la operación de modificación de posición. El valor devuelto por el socket indica el resultado de la operación:
  - `0` significa que el componente completó correctamente la operación de modificación de posición y se ejecutaron todas las acciones planificadas.
  - `-1` indica que el componente no inició la modificación de posición por una discrepancia entre la posición actual y la condición especificada (por ejemplo, si la posición actual es distinta de cero y la condición era "OpenPosition").
  - Cualquier valor mayor que `0` indica que el proceso de modificación de posición se interrumpió antes de completarse. Esto puede ocurrir por cancelación mediante la lógica del esquema o por un error durante el registro de la orden.

## Parámetros

- **Condition**: condiciones de modificación de posición:
  - `None`: no realiza acciones.
  - `OpenPosition`: abre una posición en la dirección especificada.
  - `ClosePosition`: cierra la posición actual.
  - `Decrease`: reduce el tamaño de la posición actual.
  - `Increase`: aumenta el tamaño de la posición actual.
  - `Reverse`: cierra la posición actual y abre una nueva en la dirección opuesta.
- **Direction**: especifica la dirección para "OpenPosition" y "None", y sirve como filtro opcional para otras condiciones.
- **Algorithm**: las opciones incluyen "Market Order", "VWAP", "Iceberg".
- **Part**: fracción del volumen total que se dividirá en segmentos menores al usar algoritmos como "VWAP" o "Iceberg".

Si el componente recibe un disparador mientras ya ha empezado a cambiar el volumen, ignora el nuevo disparador. Si las condiciones de modificación son incompatibles con el estado actual de la posición (por ejemplo, intentar "OpenPosition" cuando ya hay una posición abierta), el componente devuelve inmediatamente `-1` a través del socket de salida **Balance**, indicando que la operación no es necesaria y no se inició.

## Nota

Para la gestión de órdenes de bajo nivel, puede usarse el componente [Order Registration](../orders/register.md). Para una gestión de posiciones de nivel superior, se recomienda este componente "Modify Position".

## Véase también

- [Registro de orden](../orders/register.md)
