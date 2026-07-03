# Registro de orden

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

El componente "Order Registration" se usa para colocar órdenes de trading para un instrumento seleccionado.

## Sockets de entrada

- **Instrument** – instrumento seleccionado para la orden.
- **Price** – especifica el precio de una orden limitada.
- **Trigger** – señal de activación para la orden; acepta cualquier valor excepto `False`.
- **Volume** – cantidad de instrumentos para la orden.
- **Portfolio** – cartera dentro de la cual se colocará la orden.

## Sockets de salida

- **Order** – información sobre la orden colocada.
- **Error** – información sobre cualquier error durante el registro de la orden.
- **Transaction** – información sobre la transacción realizada sobre la orden.
- **Cancellation** – señal de que la orden fue cancelada.
- **Executed** – señal de que la orden fue completamente ejecutada.
- **Completed** – señal que combina eventos de error, cancelación o ejecución completa de la orden.

## Parámetros

- **Direction** – determina si la orden es de compra o venta.
- **Market Order** – indica si la orden es de mercado.
- **Zero Price** – si el precio se establece en cero, la orden se registra como orden de mercado.
- **Lifetime** – duración durante la cual una orden limitada permanece activa.

## Configuración de orden condicional

**Conditional Order** – orden con condiciones adicionales que determinan el momento de colocación en el sistema de trading según la situación actual del mercado.

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** – conexión donde se colocará la orden.
- **Stop Order Type** – tipo de orden stop.
- **Result** – resultado de la orden stop ejecutada.
- **Instrument Identifier** – identificador del instrumento para órdenes stop con condiciones relacionadas con otro instrumento.
- **Stop Price Condition** – condición de precio stop. Se usa para órdenes como "Stop price for another instrument."
- **Stop Price** – precio stop que establece la condición para activar la orden stop.
- **Stop-Limit Price** – similar al Stop Price, pero se usa solo para órdenes de tipo "Take-profit and stop-limit".
- **Stop-Limit at Market Price** – indica si la orden "Stop-Limit" se ejecuta al precio de mercado.
- **Condition Check Interval** – intervalo de tiempo para comprobar las condiciones de la orden solo dentro del período especificado (si es null, no hay comprobaciones). Se usa para tipos "Take-profit and stop-limit" y "Take-profit and stop-limit by order".
- **Conditional Order Execution Identifier** – identificador de la orden condicional basada en ejecución.
- **Direction of Conditional Order by Execution** – dirección de la orden condicional basada en ejecución.
- **Activation on Partial Execution** – se tiene en cuenta la ejecución parcial de la orden. Una orden "on-execution" se activará tras la ejecución parcial de la orden de condición.
- **Executed Volume** – toma el volumen ejecutado de la orden como cantidad para colocar la orden stop. La cantidad de valores en una orden "on-execution" se toma como el volumen ejecutado de la orden de condición.
- **Price of Linked Order** – precio de la orden limitada vinculada.
- **Withdrawal on Partial Execution** – indica la retirada de la orden stop tras la ejecución parcial de la orden limitada vinculada.
- **Offset from Maximum** – cantidad de desplazamiento desde el precio máximo (mínimo) de la última transacción.
- **Protective Spread** – tamaño del spread protector.
- **Take-Profit at Market Price** – indica si la orden "Take-Profit" se ejecuta al precio de mercado.

## Nota

Trabajar con órdenes es un método de bajo nivel para gestionar posiciones. Para una gestión de nivel superior, se recomienda usar el componente "Modify Position", descrito en [Modificar posición](../positions/modify.md).

## Véase también

[Modificar posición](../positions/modify.md)
